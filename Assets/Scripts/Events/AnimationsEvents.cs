using BaseScripts;
using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    public void SwordColliderDisable()
    {
        StartScript.GetStartScript.SwordStartController.swordCollider.enabled = false;
    }

    public void SwordColliderEnable()
    {
        StartScript.GetStartScript.SwordStartController.swordCollider.enabled = true;
    }

    public void NormalAttackEnd()
    {
        StartScript.GetStartScript.AnimController.NormalAttackInPlay = true;
        StartScript.GetStartScript.AnimController.HeavyAttackInPlay = true;
    }
    public void NormalAttackStart()
    {
        StartScript.GetStartScript.AnimController.NormalAttackInPlay = false;
        StartScript.GetStartScript.AnimController.HeavyAttackInPlay = false;
    }

    public void HeavyAttackEnd()
    {
        StartScript.GetStartScript.AnimController.HeavyAttackInPlay = true;
        StartScript.GetStartScript.AnimController.NormalAttackInPlay = true;
    }
    public void HeavyAttackStart()
    {
        StartScript.GetStartScript.AnimController.HeavyAttackInPlay = false;
        StartScript.GetStartScript.AnimController.NormalAttackInPlay = false;
    }
}
