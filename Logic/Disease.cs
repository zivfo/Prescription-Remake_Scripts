namespace RefinedGame.Logic
{
    using RefinedGame.Data;

    public class Disease
    {
        public DiseaseData data;

        public Disease(DiseaseData data)
        {
            this.data = data;
        }

        public void FormName()
        {
            LocalizableString adj = new LocalizableString(data.adj);
            LocalizableString organ = new LocalizableString(data.organ);
            LocalizableString noun = new LocalizableString("Diagnosis_disease");

            data.theName = adj.ToString() + organ.ToString() + noun.ToString();
        }
    }
}

