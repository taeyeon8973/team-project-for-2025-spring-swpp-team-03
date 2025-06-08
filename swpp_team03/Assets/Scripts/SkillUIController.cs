using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillUIController : MonoBehaviour
{
    public Button button_h;
    public Button button_j;
    public Button button_k;
    public Button button_l;
    
    // Command Pattern 적용
    private Dictionary<string, ISkillCommand> skillCommands;

	public SkillCooldown cooldown_h;
	public SkillCooldown cooldown_j;
	public SkillCooldown cooldown_k;
	public SkillCooldown cooldown_l;

    void Start()
    {
        // 스킬 명령 초기화
        InitializeSkillCommands();
        
        button_h.onClick.AddListener(() => UseSkill("H"));
        button_j.onClick.AddListener(() => UseSkill("J"));
        button_k.onClick.AddListener(() => UseSkill("K"));
        button_l.onClick.AddListener(() => UseSkill("L"));
    }
    
    void InitializeSkillCommands()
    {
        skillCommands = new Dictionary<string, ISkillCommand>
        {
            { "h", new DragonSkillCommand() },   // 청룡
            { "j", new TigerSkillCommand() },    // 백호
            { "k", new PhoenixSkillCommand() },  // 주작
            { "l", new TurtleSkillCommand() }    // 현무
        };
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

        string lowerKey = key.ToLower();
		if (lowerKey == "h")
		{
			if (cooldown_h.IsOnCooldown())
			{
				Debug.Log("h : 쿨다운중");
				return;
			}
			cooldown_h.cooldownDuration = 10;
			cooldown_h.TriggerCooldown();
		} 
		if (lowerKey == "j")
		{
			if (cooldown_j.IsOnCooldown())
			{
				Debug.Log("j : 쿨다운중");
				return;
			}
			cooldown_j.cooldownDuration = 12;
			cooldown_j.TriggerCooldown();
		} 
		if (lowerKey == "k")
		{
			if (cooldown_k.IsOnCooldown())
			{
				Debug.Log("k : 쿨다운중");
				return;
			}
			cooldown_k.cooldownDuration = 8;
			cooldown_k.TriggerCooldown();
		} 
		if (lowerKey == "l")
		{
			if (cooldown_l.IsOnCooldown())
			{
				Debug.Log("l : 쿨다운중");
				return;
			}
			cooldown_l.cooldownDuration = 15;
			cooldown_l.TriggerCooldown();
		} 
		
        
        if (skillCommands.ContainsKey(lowerKey))
        {
            ISkillCommand command = skillCommands[lowerKey];
            if (command.CanExecute())
            {
                command.Execute();
                
                // Observer Pattern - 스킬 사용 이벤트 발생
                GameEventSystem.Instance.TriggerEvent(GameEventType.SkillUsed, new { 
                    skillName = command.GetSkillName(), 
                    skillKey = key,
                    timestamp = Time.time 
                });
            }
            else
            {
                Debug.LogWarning($"🚫 {command.GetSkillName()}을(를) 사용할 수 없습니다!");
            }
        }
        else
        {
            Debug.Log("💥 Invalid skill key!");
        }
    }

    // 기존 메서드들은 하위 호환성을 위해 유지 (사용되지 않음)
    void skill_H()
    {

        // 청룡
		if (cooldown_h.IsOnCooldown())
		{
			Debug.Log("쿨다운중");
			return;
		}


        Debug.Log("근처 장애물 파괴!");
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            AreaDestroy areaDestroy = player.GetComponent<AreaDestroy>();
            if (areaDestroy != null)
            {
                areaDestroy.ManualTrigger();
				cooldown_h.cooldownDuration = 10;
				cooldown_h.TriggerCooldown();
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
		if (cooldown_j.IsOnCooldown())
		{
			Debug.Log("쿨다운중");
			return;
        }

		Debug.Log("🐯 백호 스킬 발동: 돌진!");


        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            DashForward dash = player.GetComponent<DashForward>();
            if (dash != null)
            {
                dash.StartDash();
				cooldown_j.cooldownDuration = 12;
				cooldown_j.TriggerCooldown();
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
		if (cooldown_k.IsOnCooldown())
		{
			Debug.Log("쿨다운중");
			return;
        }

        Debug.Log("점프!");
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            HighJump jumpSkill = player.GetComponent<HighJump>();
            if (jumpSkill != null)
            {
                jumpSkill.ManualTrigger();
				cooldown_k.cooldownDuration = 8;
				cooldown_k.TriggerCooldown();
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
		if (cooldown_l.IsOnCooldown())
		{
			Debug.Log("쿨다운중");
			return;
        }
        Debug.Log("현무모드");

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            HyunmuMode hyunmu = player.GetComponent<HyunmuMode>();
            if (hyunmu != null)
            {
                hyunmu.ManualTrigger();
				cooldown_l.cooldownDuration = 20;
				cooldown_l.TriggerCooldown();
            }
            else
            {
                Debug.LogWarning("🚫 HyunmuMode 스크립트가 없음!");
            }
        }
    }
}
