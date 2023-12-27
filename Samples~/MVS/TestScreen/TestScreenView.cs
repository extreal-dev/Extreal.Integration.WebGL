﻿using System;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Extreal.Integration.Web.Common.MVS.TestScreen
{
    public class TestScreenView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField param1InputField;
        [SerializeField] private TMP_InputField param2InputField;
        [SerializeField] private Button actionButton;
        [SerializeField] private Button functionButton;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button suppressDoFunctionTraceLogButton;

        public class Parameters
        {
            public string Param1 { get; private set; }
            public string Param2 { get; private set; }

            public Parameters(string param1, string param2)
            {
                Param1 = param1;
                Param2 = param2;
            }
        }

        public IObservable<Parameters> OnActionButtonClicked => onActionButtonClicked;
        [SuppressMessage("CodeCracker", "CC0033")]
        private readonly Subject<Parameters> onActionButtonClicked = new Subject<Parameters>();

        public IObservable<Parameters> OnFunctionButtonClicked => onFunctionButtonClicked;
        [SuppressMessage("CodeCracker", "CC0033")]
        private readonly Subject<Parameters> onFunctionButtonClicked = new Subject<Parameters>();

        public IObservable<Unit> OnSuppressDoFunctionTraceLogButtonClicked => onSuppressDoFunctionTraceLogButtonClicked;
        [SuppressMessage("CodeCracker", "CC0033")]
        private readonly Subject<Unit> onSuppressDoFunctionTraceLogButtonClicked = new Subject<Unit>();

        [SuppressMessage("CodeCracker", "CC0068")]
        private void Awake()
        {
            actionButton
                .OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ => onActionButtonClicked.OnNext(new Parameters(param1InputField.text, param2InputField.text)));

            functionButton
                .OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ => onFunctionButtonClicked.OnNext(new Parameters(param1InputField.text, param2InputField.text)));

            suppressDoFunctionTraceLogButton
                .OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    suppressDoFunctionTraceLogButton.interactable = false;
                    onSuppressDoFunctionTraceLogButtonClicked.OnNext(Unit.Default);
                });
        }

        [SuppressMessage("CodeCracker", "CC0068")]
        private void OnDestroy()
        {
            onActionButtonClicked.Dispose();
            onFunctionButtonClicked.Dispose();
            onSuppressDoFunctionTraceLogButtonClicked.Dispose();
        }

        public void ShowResult(string result) => resultText.text = result;
    }
}
