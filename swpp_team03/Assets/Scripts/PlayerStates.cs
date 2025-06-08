using UnityEngine;

// 일반 상태
public class NormalState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        Debug.Log("Normal State 진입");
    }

    public void Update(PlayerController player)
    {
        // 일반 상태에서의 로직 (기존 PlayerController 로직)
    }

    public void Exit(PlayerController player)
    {
        Debug.Log("Normal State 종료");
    }

    public string GetStateName()
    {
        return "Normal";
    }
}

// 대시 상태
public class DashingState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        Debug.Log("Dashing State 진입");
    }

    public void Update(PlayerController player)
    {
        // 대시 중에는 일반 이동 제한
    }

    public void Exit(PlayerController player)
    {
        Debug.Log("Dashing State 종료");
    }

    public string GetStateName()
    {
        return "Dashing";
    }
}

// 무적 상태
public class InvincibleState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        Debug.Log("Invincible State 진입");
    }

    public void Update(PlayerController player)
    {
        // 무적 상태 로직
    }

    public void Exit(PlayerController player)
    {
        Debug.Log("Invincible State 종료");
    }

    public string GetStateName()
    {
        return "Invincible";
    }
}

// 면역 상태
public class ImmuneState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        Debug.Log("Immune State 진입");
    }

    public void Update(PlayerController player)
    {
        // 면역 상태 로직
    }

    public void Exit(PlayerController player)
    {
        Debug.Log("Immune State 종료");
    }

    public string GetStateName()
    {
        return "Immune";
    }
} 