using System.Collections;
using UnityEngine;

namespace SuperBilliard
{
    public class FancyBilliardLogic : BilliardLogicBase
    {

        protected override void OnCollisionEnter(Collision collision)
        {
            //不等于-1说明还没有参与碰撞

            if (FirestCollideId != -1)
            {
                return;
            }

            if (collision.gameObject.CompareTag(Constant.GameobjectTag.CannotReboundTag))
            {
                FirestCollideId = -2;
            }
            else if (collision.gameObject.CompareTag(Constant.GameobjectTag.BallTag))
            {
                FirestCollideId = collision.gameObject.GetComponent<IBilliard>().BilliardId;
            }

            //如果没有处于同步状态,则播放音效
            //if (_isSyncing == false)
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
            float timer = 0;
            _hiding = true;
            _rigidbody.velocity = Vector3.zero;

            //进球
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
                BilliardManager.Instance.RemoveUsingBilliard(this);
            }
        }
    }
}