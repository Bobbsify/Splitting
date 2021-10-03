using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StateControllerInterface
{
    void DisableControl();

    void EnableControl();

    void DisableJump();

    void EnableJump();

    void DisablePush();

    void EnablePush();
}
