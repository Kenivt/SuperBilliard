using UnityEngine;
using UnityGameFramework;

namespace SuperBilliard
{
    [LoaclizationEnumText]
    public enum LoginPrompt
    {
        [LoaclizationEnumAdditional("The length of the user name must be 12.")]
        UserNameLenghtLimit,

        [LoaclizationEnumAdditional("Please enter your username.")]
        PleaseEnterPassword,

        [LoaclizationEnumAdditional("The two passwords are different.")]
        ConfirmPasswordErrored,

        [LoaclizationEnumAdditional("Wrong password.")]
        WrongPassword,

        [LoaclizationEnumAdditional("Fail to sign in.The current user name has been occupied.")]
        RegisterFail,

        [LoaclizationEnumAdditional("Registration successful, please login again.")]
        RegisterSuccess,
    }
}