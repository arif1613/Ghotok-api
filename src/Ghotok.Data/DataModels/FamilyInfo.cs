using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ghotok.Data.DataModels
{
    public class FamilyInfo
    {
        [Key]
        public Guid Id { get; set; }
        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
    }

    public class FamilyMember
    {
        [Key]
        public Guid Id { get; set; }
        public string FamilyMemberName { get; set; }
        public string FamilyMemberOccupation { get; set; }
        public string Relationship { get; set; }
    }

    public enum FamilyMemberRelationShip
    {
        Father,
        Mother,
        Brother,
        Sister,
        GrandFatherMaternal,
        GrandFatherPaternal,
        GrandMotherMaternal,
        GrandMotherPaternal,
        UncleMaternal,
        UnclePaternal,
        AuntyMaternal,
        AuntyPaternal,
        FamilyFriend,
        Other
    }
}
