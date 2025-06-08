using System;
using System.Collections.Generic;
using UnityEngine;

// 게임 이벤트 타입
public enum GameEventType
{
    PlayerHealthChanged,
    PlayerEnergyChanged,
    PlayerStateChanged,
    SkillUsed,
    EnemyDefeated,
    GameOver
}

// 이벤트 데이터 클래스
[System.Serializable]
public class GameEventData
{
    public GameEventType eventType;
    public object data;
    public float timestamp;
    
    public GameEventData(GameEventType type, object eventData = null)
    {
        eventType = type;
        data = eventData;
        timestamp = Time.time;
    }
}

// Observer 인터페이스
public interface IGameEventObserver
{
    void OnGameEvent(GameEventData eventData);
}

// 이벤트 시스템 (Singleton 패턴 적용)
public class GameEventSystem : MonoBehaviour
{
    private static GameEventSystem instance;
    public static GameEventSystem Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameEventSystem");
                instance = go.AddComponent<GameEventSystem>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    
    private Dictionary<GameEventType, List<IGameEventObserver>> observers = new Dictionary<GameEventType, List<IGameEventObserver>>();
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    // 옵저버 등록
    public void Subscribe(GameEventType eventType, IGameEventObserver observer)
    {
        if (!observers.ContainsKey(eventType))
        {
            observers[eventType] = new List<IGameEventObserver>();
        }
        
        if (!observers[eventType].Contains(observer))
        {
            observers[eventType].Add(observer);
        }
    }
    
    // 옵저버 해제
    public void Unsubscribe(GameEventType eventType, IGameEventObserver observer)
    {
        if (observers.ContainsKey(eventType))
        {
            observers[eventType].Remove(observer);
        }
    }
    
    // 이벤트 발생
    public void TriggerEvent(GameEventType eventType, object data = null)
    {
        GameEventData eventData = new GameEventData(eventType, data);
        
        if (observers.ContainsKey(eventType))
        {
            foreach (var observer in observers[eventType])
            {
                observer.OnGameEvent(eventData);
            }
        }
        
        // 디버그 로그
        Debug.Log($"🎯 Event Triggered: {eventType} at {eventData.timestamp}");
    }
    
    // 모든 옵저버 해제
    public void UnsubscribeAll(IGameEventObserver observer)
    {
        foreach (var kvp in observers)
        {
            kvp.Value.Remove(observer);
        }
    }
} 