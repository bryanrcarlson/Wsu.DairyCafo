namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class ManureStorage : Entity
    {
        public double SurfaceArea_m2 { get; set; }
        public double VolumeMax_m3 { get; set; }
        public double PH_mol_L { get; set; }
        public string Style { get; set; }
        public bool DoesContainFreshManure { get; set; }
    }
}
