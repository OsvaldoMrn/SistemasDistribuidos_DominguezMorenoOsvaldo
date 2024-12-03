using System.Runtime.Serialization;

namespace RestApi.Infrastructure.Soap;


[DataContract]
public class UserResponseDto
{
    [DataMember]
    public Guid UserId {get; set; }
    [DataMember]
    public string Email {get; set;} = null!;
    [DataMember]
    public string FirstName {get; set; } = null!;
    [DataMember]
    public string LastName {get; set; } = null!;
    [DataMember]
    public DateTime BirthDate {get; set; }

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SoapApi.Dtos")]
public class UserResponseDto
{
    [DataMember(Order = 5)]
    public Guid UserId { get; set; }
    
    [DataMember(Order = 2)]
    public string Email { get; set; } = null!;
    
    [DataMember(Order = 3)]
    public string FirstName { get; set; } = null!;
    
    [DataMember(Order = 4)]
    public string LastName { get; set; } = null!;
    
    [DataMember(Order = 1)]
    public DateTime BirthDate { get; set; }
}