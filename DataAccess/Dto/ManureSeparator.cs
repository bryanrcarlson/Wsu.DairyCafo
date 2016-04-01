namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class ManureSeparator : Entity
    {
        public string Style { get; set; }
        public string SourceFacility { get; set; }
        public string LiquidFacility { get; set; }
        public string SolidFacility { get; set; }

        public ManureSeparator()
        {
            SourceFacility = "";
            LiquidFacility = "";
            SolidFacility = "";
        }
        protected virtual void Copy(ManureSeparator toCopy)
        {
            this.Id = toCopy.Id;
            this.Enabled = toCopy.Enabled;
            this.Style = toCopy.Style;
            this.SourceFacility = toCopy.SourceFacility;
            this.LiquidFacility = toCopy.LiquidFacility;
            this.SolidFacility = toCopy.SolidFacility;
        }
    }
}
