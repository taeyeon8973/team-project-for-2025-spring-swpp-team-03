using UnityEngine;

public interface ISkillCommand
{
    void Execute();
    bool CanExecute();
    string GetSkillName();
} 