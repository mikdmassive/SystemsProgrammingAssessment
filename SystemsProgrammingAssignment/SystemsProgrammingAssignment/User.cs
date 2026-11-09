public class User
{
    private string UserID;
    private string Username;
    private string PasswordHash;
    private int AccessLevel;
    private int MembershipLevel;
    public User (string UserID,string Username,string PasswordHash,int AccessLevel,int MembershipLevel)
    {
        this.UserID = UserID;
        this.Username = Username;
        this.PasswordHash = PasswordHash;
        this.MembershipLevel = MembershipLevel;
        this.AccessLevel = AccessLevel;
    }
    public string ReturnUserID() {  return UserID; }
    public string ReturnUsername() {  return Username; }
    public int ReturnMembershipLevel() { return MembershipLevel; }

}