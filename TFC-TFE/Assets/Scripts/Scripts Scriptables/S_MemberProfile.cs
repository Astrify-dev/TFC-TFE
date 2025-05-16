using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "NewMemberProfile", menuName = "ScriptableObjects/MemberProfile", order = 1)]
public class MemberProfile : ScriptableObject{
    public Sprite leftImage;

    [TextArea] public string MemberName;

    [TextArea] public string MemberRole;

    public Sprite rightImage;
}
