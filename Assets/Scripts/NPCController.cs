using UnityEngine;

public class NPCController : Character
{
    protected override InputDataDto GetInputParameters()
    {
        return new InputDataDto(Random.Range(0f, 1f), Random.Range(0f, 1f), false);
    }
}