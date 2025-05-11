using System.Collections;
using UnityEngine;

public class DefaultBlock : Block
{
    public override IEnumerator OnSelected(MatchManager manager)
    {
        yield return manager.HandleMatchSequence(this);
    }
}
