namespace Acklann.MailN
{
    public interface IContact
    {
        string FirstName { get; }

        string MiddleName { get; }

        string LastName { get; }

        string Email { get; }
    }
}