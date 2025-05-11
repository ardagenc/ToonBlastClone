using System.Collections;
using UnityEngine;

public class JuiceBlock : Block
{
    public override IEnumerator OnSelected(MatchManager manager)
    {
        yield return manager.HandleJuiceSequence(this);
    }
}
