using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class S_MembersUI : MonoBehaviour{
    [SerializeField] private Image leftImageUI;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Role;
    [SerializeField] private Image RightImage;

    [SerializeField] private MemberProfile Member;

    private void Start(){
        if (Member is not null){
            leftImageUI.sprite = Member.leftImage;
            Name.text = Member.MemberName;
            Role.text = Member.MemberRole;
            RightImage.sprite = Member.rightImage;
        }
    }
}
