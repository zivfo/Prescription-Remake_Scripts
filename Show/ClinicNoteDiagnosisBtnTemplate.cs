using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClinicNoteDiagnosisBtnTemplate : MonoBehaviour
{
    [SerializeField] TMP_Text diseaseName = null;
    [SerializeField] RectTransform diagnosedSignObj = null;
    [SerializeField] Button btn = null;

    System.Guid thisDiseaseId;

    public void Init(Disease disease, Action<System.Guid> onDiseaseSelected)
    {
        if (disease == null)
        {
            this.thisDiseaseId = Guid.Empty;
            diseaseName.text = "No Diagnosis";
        }
        else
        {
            this.thisDiseaseId = disease.data.id;
            diseaseName.text = disease.data.theName;
        }

        diagnosedSignObj.gameObject.SetActive(false);
        btn.onClick.AddListener(() =>
        {
            diagnosedSignObj.DOKill();
            diagnosedSignObj.DOPunchAnchorPos(Vector2.right * 10, 0.3f, 0);
            onDiseaseSelected(thisDiseaseId);
            diagnosedSignObj.gameObject.SetActive(true);
        });
    }

    internal void ResetBtnSelection()
    {
        diagnosedSignObj.gameObject.SetActive(false);
    }
}
