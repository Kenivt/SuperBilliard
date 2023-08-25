using UnityEngine;
using System.Collections;

namespace SuperBilliard
{
    public class SnokkerBilliardLogic : BilliardLogicBase
    {
        //斯诺克台球碰撞墙壁不会扣分
        //-1为没有碰撞到任何物体,其他的为对应球的id
        protected override void OnCollisionEnter(Collision collision)
        {
            if (FirestCollideId != -1)
            {
                return;
            }

            if (collision.gameObject.CompareTag(Constant.GameobjectTag.BallTag))
            {
                FirestCollideId = collision.gameObject.GetComponent<IBilliard>().BilliardId;
            }

            float volume = Mathf.Clamp01(Velocity.magnitude / 30f);
            if (volume > 0.05f)
            {
                GameEntry.Sound.PlaySound(EnumSound.BilliardCollide, transform.position, volume);
                GameEntry.Client.SendSyncSoundMessage(EnumSound.BilliardCollide, volume, transform.position);
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constant.GameobjectTag.BallHoleTag))
            {
                StopAllCoroutines();
                StartCoroutine(HideBilliard(other.transform.position));
            }
        }

        protected IEnumerator HideBilliard(Vector3 holePos)
        {
            _rigidbody.velocity = Vector3.zero;
            float timer = 0;

            //进球
            _hiding = true;
            GameEntry.Event.FireNow(this, BilliardGoalEventArgs.Create(this));

            while (timer < _hideTime)
            {
                timer += Time.deltaTime;
                float fillamount = Mathf.Clamp01(timer / _hideTime);
                transform.position = Vector3.Lerp(transform.position, holePos, fillamount);
                yield return null;
            }

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _hiding = false;

            if (BilliardId == 0)
            {
                gameObject.transform.position = LevelManager.Instance.DefaultPos;
                gameObject.SetActive(false);
            }
            else
            {
                SetActive(false);
                BilliardManager.Instance.RemoveUsingBilliard(this);
            }
        }

    }
}
