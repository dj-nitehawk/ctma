namespace Dom;

sealed class Member : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Designation { get; set; }
    public string CurrentWork { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDay { get; set; }
    public string Gender { get; set; }
    public string Nic { get; set; }
    public string Slmc { get; set; }
    public string MobileNumber { get; set; }
    public bool Whatsapp { get; set; }
    public bool Viber { get; set; }
    public bool Telegram { get; set; }
    public Address Address { get; set; }
    public string Qualifications { get; set; }
    public string Collaborate { get; set; }
    public DateOnly SignupDate { get; set; }
}