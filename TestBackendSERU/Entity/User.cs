
namespace TestBackendSERU.Entity 
{
    public class User : Audit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Is_Admin { get; set; }
    }
}