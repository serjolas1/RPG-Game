﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private bool isTellegiated = false;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 20.0f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 0.0f;
        [SerializeField] private UnityEvent onLaunch = null;
        [SerializeField] private UnityEvent onHit    = null;
        Health target = null;
        GameObject instigator;
        float damage = 0.0f;

        private void Start()
        {
            Destroy(gameObject, maxLifeTime);

            if (target == null) return;
            transform.LookAt(GetAimLocation());
            onLaunch?.Invoke();
        }

        void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;

            if (target == null) return;

            if (isTellegiated && target != null)
            {
                transform.LookAt(GetAimLocation());
            }

            if (target.GetComponent<Health>().IsDead())
            {
                target = null;
            }
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        private Vector3 GetAimLocation()
        {
            return target.transform.position + Vector3.up * target.GetComponent<CapsuleCollider>().height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target == null && other.GetComponent<Health>() != null)
            {
                other.GetComponent<Health>().TakeDamage(instigator, damage);
                speed = 0;
                DestroySelf();

                return;
            }

            if (other.GetComponent<Health>() != target || other.GetComponent<Health>() == null) return;
            target.TakeDamage(instigator, damage);
            speed = 0;
            DestroySelf();
        }

        void DestroySelf()
        {
            if (hitEffect != null) Instantiate(hitEffect, transform.position, transform.rotation);

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
            onHit?.Invoke();
        }
    }

}