namespace Mawthooq.Domain.SharedKernel.Enums
{
    [Flags]
    public enum PhoneType : byte
    {
        Primary = 1,
        WhatsApp = 2,
        Secondary = 3,
        PrimaryAndWhatsApp = 4,
        Fax = 5
    }
}
