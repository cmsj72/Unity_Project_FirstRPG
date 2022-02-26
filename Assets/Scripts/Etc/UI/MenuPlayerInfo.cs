using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerInfo : MonoBehaviour
{
    private PlayerInfo playerinfo;

    //이름
    private UILabel NameLabel;
    private UILabel PlayerNameLabel;

    //클래스
    private UILabel ClassLabel;
    private UILabel PlayerClassLabel;

    //레벨
    private UILabel LevelLabel;
    private UILabel PlayerLevelLabel;

    //HP
    private UILabel HPLabel;
    private UILabel PlayerHPLabel;

    //MP
    private UILabel MPLabel;
    private UILabel PlayerMPLabel;

    //EXP
    private UILabel EXPLabel;
    private UILabel PlayerEXPLabel;

    // Start is called before the first frame update
    void Start()
    {
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        Transform menuchild = GameObject.Find("MenuChild").transform;

        NameLabel = menuchild.Find("CharacterInfoButton/Info/MenuInfoName").GetComponent<UILabel>();
        PlayerNameLabel = menuchild.Find("CharacterInfoButton/Info/PlayerName").GetComponent<UILabel>();

        ClassLabel = menuchild.Find("CharacterInfoButton/Info/MenuInfoClass").GetComponent<UILabel>();
        PlayerClassLabel = menuchild.Find("CharacterInfoButton/Info/PlayerClass").GetComponent<UILabel>();

        LevelLabel = menuchild.Find("CharacterInfoButton/Info/MenuInfoLevel").GetComponent<UILabel>();
        PlayerLevelLabel = menuchild.Find("CharacterInfoButton/Info/PlayerLevel").GetComponent<UILabel>();

        HPLabel = menuchild.Find("CharacterInfoButton/Info/MenuInfoHP").GetComponent<UILabel>();
        PlayerHPLabel = menuchild.Find("CharacterInfoButton/Info/PlayerHP").GetComponent<UILabel>();

        MPLabel = menuchild.Find("CharacterInfoButton/Info/MenuInfoMP").GetComponent<UILabel>();
        PlayerMPLabel = menuchild.Find("CharacterInfoButton/Info/PlayerMP").GetComponent<UILabel>();

        EXPLabel = menuchild.Find("CharacterInfoButton/Info/MenuInfoEXP").GetComponent<UILabel>();
        PlayerEXPLabel = menuchild.Find("CharacterInfoButton/Info/PlayerEXP").GetComponent<UILabel>();

        NameLabel.fontSize = NameLabel.height - 10;
        PlayerNameLabel.fontSize = NameLabel.fontSize;

        ClassLabel.fontSize = ClassLabel.height - 10;
        PlayerClassLabel.fontSize = ClassLabel.fontSize;

        LevelLabel.fontSize = ClassLabel.fontSize;
        PlayerLevelLabel.fontSize = ClassLabel.fontSize;

        HPLabel.fontSize = ClassLabel.fontSize;
        PlayerHPLabel.fontSize = ClassLabel.fontSize;

        MPLabel.fontSize = ClassLabel.fontSize;
        PlayerMPLabel.fontSize = ClassLabel.fontSize;

        EXPLabel.fontSize = ClassLabel.fontSize;
        PlayerEXPLabel.fontSize = ClassLabel.fontSize;

        NameLabel.text = "Name";
        ClassLabel.text = "Class";
        LevelLabel.text = "Level";
        HPLabel.text = "HP";
        MPLabel.text = "MP";
        EXPLabel.text = "EXP";

        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerNameLabel.text = playerinfo.GetPlayerName();
        PlayerClassLabel.text = playerinfo.GetPlayerClass();
        PlayerLevelLabel.text = playerinfo.GetPlayerLevel().ToString();
        PlayerHPLabel.text = playerinfo.GetPlayerCurHP().ToString() + " / " + playerinfo.GetPlayerMaxHP().ToString();
        PlayerMPLabel.text = playerinfo.GetPlayerCurMP().ToString() + " / " + playerinfo.GetPlayerMaxMP().ToString();
        PlayerEXPLabel.text = playerinfo.GetPlayerCurEXP().ToString() + " / " + playerinfo.GetPlayerMaxEXP().ToString();
    }
}
