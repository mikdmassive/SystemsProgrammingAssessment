public class MembershipManager 
{
    public Dictionary<int, Membership> memberships = new Dictionary<int, Membership>() {
        {1,new Membership(1,0f,"Bronze","The free membership, allows access to basic vehicles")},
        {2,new Membership(1,20f,"Silver","The medium membership, allows access to some of our more premium vehicles.")},
        {3,new Membership(3,40.75f,"Gold","The best membership, allows access all of the vehicles we provide.")},
    };
    
}