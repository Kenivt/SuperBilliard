using UnityEngine;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class CoroutineTest : MonoBehaviour
    {
        MyCoroutine myCoroutine = new MyCoroutine();
        IEnumerator<IMyCoroutineItem> _enumerator;

        private void Start()
        {
            _enumerator = enumerator();
            myCoroutine.StartCoroutine(_enumerator);
            object obj = enumerator();
        }

        private void Update()
        {
            myCoroutine.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        IEnumerator<IMyCoroutineItem> enumerator()
        {
            float count = 0;
            while (true)
            {
                yield return new MyCorotineWaitSecond(1);
                Debug.Log(count++);
                if (count == 10)
                {
                    break;
                }
            }
            yield return new MyCoroutineWaitUtill(() =>
            {
                return Input.GetKeyDown(KeyCode.Space);
            });
            Debug.Log("Space");
        }
    }
}