﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesciptionData : MonoBehaviour
{
    Dictionary<int, string[]> desData;

    private void Awake()
    {
        desData = new Dictionary<int, string[]>();
        GenerateData();
    }

    private void GenerateData()
    {
        desData.Add(1, new string[]
        { "저 <color=green>우편물</color>에는\n빠르게 달리게 하는 힘이 있어","한 번 획득해보자!"});
        desData.Add(2, new string[] { "<b>저기 높낮이가 \n다른 지형이 있네</b>",
            "<color=green>점프</color>를 해서 올라가보자", "점프는 <color=green>Space</color>키로 \n할 수 있어!"});
        desData.Add(3, new string[] { "잘했어!\n자 이제 <color=green>이단 점프</color>를 배워볼거야",
            "점프 중에  <color=green>한 번 더 \nSpace</color>키를 입력해봐!"});
        desData.Add(4, new string[] { "엇 저기 <color=green>마법진</color>이 보이네",
            "마법진을 통해서\n우편물을 배달할 수 있어","마법진 위에서 \n<color=green>왼쪽 Shift</color>키를 누르면\n배달 될 거야",
            "마법진 <color=green>범위 안</color>이라면\n<color=green>공중</color>에서도 배달 할 수 있으니\n기억해둬!" });
        desData.Add(5, new string[] { "<color=green>우편물 1개가 소모</color>되면서\n오른쪽 게이지가 <color=green>상승</color>했을거야", "게이지가 <color=green>60%</color>를 넘겨야","<color=green>스테이지를 클리어</color> 할 수 있어",
            "또 마법진이 있어!\n한 번 더 해봐!"});
        desData.Add(6, new string[] { "저 앞에 길을 막은\n<color=brown>장애물</color>이 있어",
            "장애물에 부딪히면<color=brown> 일정비율의\n 우편물이 감소</color>될거야", "우편물이 감소되면\n<color=brown>속도가 느려지니</color> 조심해!",
            "우편물이 <color=brown>0개 일 때는\n특히 더 조심</color>해야 할거야.","배달 할 물건이 없는데 배달하는 의미는 없으니까"});
        desData.Add(7, new string[] { "배우느라 정말 수고했어!", "모든 준비가 끝났으니\n이제 <size=40>본격적</size>으로 달려볼까!?" });
        desData.Add(8, new string[] { "앞에 <color=green>물약</color>이 보여?\n저건 꼭 획득해야해", "살이 잠시동안 없어지는\n마법의 약이라고!","일정시간 동안 살이 빠져\n<color=green>빠른 속도</color>로 훨훨 \n날아다닐 수 있을거야!",
            "그 시간동안 배달은\n<color=green>모두 성공</color>할 수 있으니\n걱정하지 말고!" });
    }

    public string GetDesciption(int id, int DesIndex)
    {
        // 남아있는 문장이 없으면
        if (DesIndex == desData[id].Length)
            return null;
        else
           return desData[id][DesIndex];
    }

    public string questName;
    public int[] npcId;

}
