using System;
using System.Collections;
using System.Collections.Generic;
using RefinedGame.Data;
using RefinedGame.Logic;
using UnityEngine;


namespace RefinedGame.Show
{
    [Serializable]
    public class PatientStatusSprite
    {
        public Data.AbilityData.Status status;
        public Sprite sprite;
    }
    [Serializable]
    public class PatientLevelColor
    {
        public int level;
        public Color color;
    }
    public class PatientVisualController : MonoBehaviour
    {
        public class PatientAppearanceData
        {
            public System.Guid id;

            public Sprite hand;
            public Sprite body;
            public Sprite head;
            public Sprite mouth;
            public Sprite eye;
            public Sprite hair;
        }

        [SerializeField] List<Sprite> handSprites = null;
        [SerializeField] List<Sprite> bodySprites = null;
        [SerializeField] List<Sprite> headSprites = null;
        [SerializeField] List<Sprite> mouthSprites = null;
        [SerializeField] List<Sprite> eyeSprites = null;
        [SerializeField] List<Sprite> hairSprites = null;
        [SerializeField] List<PatientStatusSprite> patientStatusSigns = null;
        [SerializeField] List<PatientLevelColor> patientLevelColors = null;
        public Color deadBodyColor;
        public Color deadMouthColor;
        public Color deadEyeColor;
        public Color deadHairColor;

        List<PatientAppearanceData> currentAppearances = new List<PatientAppearanceData>();
        List<PatientAppearanceData> toRemoveAppearances = new List<PatientAppearanceData>();

        #region Unity Functions
        #endregion

        #region Public Functions
        public void Init() { }
        public System.Guid GenerateAppearance()
        {
            var data = new PatientAppearanceData();
            data.id = System.Guid.NewGuid();

            data.hand = handSprites[UnityEngine.Random.Range(0, handSprites.Count)];
            data.body = bodySprites[UnityEngine.Random.Range(0, bodySprites.Count)];
            data.head = headSprites[UnityEngine.Random.Range(0, headSprites.Count)];
            data.mouth = mouthSprites[UnityEngine.Random.Range(0, mouthSprites.Count)];
            data.eye = eyeSprites[UnityEngine.Random.Range(0, eyeSprites.Count)];
            data.hair = hairSprites[UnityEngine.Random.Range(0, hairSprites.Count)];

            currentAppearances.Add(data);
            return data.id;
        }
        public void ClearUsedAppearances()
        {
            toRemoveAppearances.Clear();
        }
        public PatientAppearanceData GetDataFromId(System.Guid id)
        {
            var data = currentAppearances.Find(x => x.id == id);
            if (data == null)
                data = toRemoveAppearances.Find(x => x.id == id);

            return data;
        }
        public Sprite GetStatusSprite(AbilityData.Status status)
        {
            return patientStatusSigns.Find(x => x.status == status).sprite;
        }
        public Color GetLevelColor(int level)
        {
            return patientLevelColors.Find(x => x.level == level).color;
        }
        public void RemoveAppearance(System.Guid id)
        {
            var appearanceData = currentAppearances.Find(x => x.id == id);
            toRemoveAppearances.Add(appearanceData);
            currentAppearances.Remove(appearanceData);
        }
        public void RemoveAllAppearances(List<System.Guid> ids)
        {
            foreach (var id in ids)
            {
                RemoveAppearance(id);
            }
        }
        #endregion

        #region Private Functions
        #endregion
    }

}
