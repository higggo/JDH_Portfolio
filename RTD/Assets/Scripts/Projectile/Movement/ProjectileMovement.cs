using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileKit
{
    //public interface ProjectileMovement
    //{
    //    void SetMovement(Transform target, float bulletSpeed);
    //}

    public class ProjectileMovement : MonoBehaviour
    {
        protected bool fireBullet = false;
        protected bool endMove = false;
        protected Transform target = null;
        protected float bulletSpeed = 0.0f;
        protected ProjectileController controller;


        protected virtual void SetMovement()
        {
            if (controller == null)
                return;

            target = controller.targetHitPoint;
            fireBullet = true;
        }

        protected virtual void SetRotate(Vector3 targetPos)
        {
            Vector3 dir = targetPos - transform.position;
            dir.Normalize();

            if (dir != Vector3.zero && transform.rotation != Quaternion.LookRotation(dir))
                transform.rotation = Quaternion.LookRotation(dir);
        }

        /// <summary>
        /// 이 함수를 상속할 클래스의 Start(), Awake()에 붙이세요.
        /// </summary>
        protected virtual void InitController()
        {
            if (GetComponent<ProjectileController>() == null)
                return;

            controller = GetComponent<ProjectileController>();
            bulletSpeed = controller.bulletSpeed;
            controller.moveDel += SetMovement;
        }
    }

}