using UnityEngine;
using UnityEngine.UI;

public class SkillUIController : MonoBehaviour
{
    public Button button_h;
    public Button button_j;
    public Button button_k;
    public Button button_l;

    void Start()
    {
        button_h.onClick.AddListener(() => UseSkill("H"));
        button_j.onClick.AddListener(() => UseSkill("J"));
        button_k.onClick.AddListener(() => UseSkill("K"));
        button_l.onClick.AddListener(() => UseSkill("L"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) UseSkill("H");
        if (Input.GetKeyDown(KeyCode.J)) UseSkill("J");
        if (Input.GetKeyDown(KeyCode.K)) UseSkill("K");
        if (Input.GetKeyDown(KeyCode.L)) UseSkill("L");
    }

    void UseSkill(string key)
    {
        Debug.Log($"💥 Skill {key} activated!");

        key = key.ToLower();

        if (key == "h")
        {
            skill_H();
        }
        else if (key == "j")
        {
            skill_J();
        }
        else if (key == "k")
        {
            skill_K();
        }
        else if (key == "l")
        {
            skill_L();
        }
        else
        {
            Debug.Log("💥 Invalid skill key!");
        }
    }

    void skill_H()
    {
        // 청룡
        Debug.Log("근처 장애물 파괴!");
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            AreaDestroy areaDestroy = player.GetComponent<AreaDestroy>();
            if (areaDestroy != null)
            {
                areaDestroy.ManualTrigger();
            }
            else
            {
                Debug.LogWarning("🚫 AreaDestroy 스크립트가 없음!");
            }
        }
    }

    void skill_J()
    {
        // 백호
            Debug.Log("🐯 백호 스킬 발동: 돌진!");

        // 플레이어에게 DashForward 컴포넌트가 있어야 함
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            DashForward dash = player.GetComponent<DashForward>();
            if (dash != null)
            {
                dash.StartDash();
            }
            else
            {
                Debug.LogWarning("🚫 DashForward 컴포넌트가 없음!");
            }
        }
    }

    void skill_K()
    {
        // 주작
        Debug.Log("점프!");
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            HighJump jumpSkill = player.GetComponent<HighJump>();
            if (jumpSkill != null)
            {
                jumpSkill.ManualTrigger();
            }
            else
            {
                Debug.LogWarning("🚫 HighJump 스크립트가 없음!");
            }
        }
    }

    void skill_L()
    {
        // 현무
        Debug.Log("🔥 불덩이 발사!");
        // 예: Instantiate(불덩이Prefab, transform.position, Quaternion.identity);
    }

}
