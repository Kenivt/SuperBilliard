using System;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class PromptMessage : IReference
    {
        public Action confirmAction;
        public string content;

        public static PromptMessage Create(Action confirm, string content)
        {
            PromptMessage promptMessage = ReferencePool.Acquire<PromptMessage>();
            promptMessage.confirmAction = confirm;
            promptMessage.content = content;
            return promptMessage;
        }

        public void Clear()
        {
            confirmAction = null;
            content = null;
        }
    }

    public class PromptUIForm : UILogicBase
    {
        [SerializeField] private Text _content;
        [SerializeField] private Button _confirmBtn;
        [SerializeField] private Button _cancleBtn;

        private PromptMessage? tipConfirm;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _confirmBtn.onClick.AddListener(Confirm);
            _cancleBtn.onClick.AddListener(Cancle);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //Action
            tipConfirm = (PromptMessage)userData;
            _content.text = tipConfirm.content;


        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            if (tipConfirm != null)
            {
                ReferencePool.Release(tipConfirm);
            }
            tipConfirm = null;
        }

        private void Confirm()
        {
            //防止多次点击
            if (Available == false)
            {
                return;
            }
            if (tipConfirm != null)
            {
                tipConfirm.confirmAction?.Invoke();
            }
            Close();
        }

        private void Cancle()
        {
            if (Available == false)
            {
                return;
            }
            Close();
        }
    }
}