# 📌 디노버거
>요리 & 시뮬레이션 게임  
>게임 이미지 스크린샷  
>https://youtube.co.kr (시연 영상)

</br>

## 1. 제작 기간 & 참여 인원
- 2025년 01월 09일 ~ 02월 27일
- 개인 프로젝트

</br>

## 2. 사용 기술
#### `Client`
- Unity6

</br>

## 3. 핵심 기능
- Car와 Guest NPC는 게임 시작 시 지정된 좌표에서 생성되어 각각의 **관리자가 제어**합니다.
  - NPC는 Waypoint로 이동하고 **State 패턴으로 상태**를 관리합니다.
    - Guest는 가게 상황에 따라 입장하거나 대기하며, 시간 내 음식을 받지 못하면 퇴장합니다.
    - Car는 **바퀴 기반으로 움직이며** 앞차 감지 시 정차합니다.

<details>
<summary><b>핵심 기능 설명 펼치기</b></summary>
<div markdown="1">

### 3.1. NPC Spawn
![Guest Spawner](https://github.com/user-attachments/assets/48a913c4-0c12-4b61-89e0-12c53683303f)

- **Spawner 초기화** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/26f141d32664c3031c122082ff2f87f32028f7fd/Assets/Scripts/Manager/Game/GuestManager.cs#L15)
  - 게임이 시작되면 미리 설정된 좌표를 불러와 생성 좌표를 초기화합니다.
- **NPC 생성** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/26f141d32664c3031c122082ff2f87f32028f7fd/Assets/Scripts/Manager/Game/GuestManager.cs#L142)
  - 일정 시간마다 지정된 좌표에 NPC를 생성합니다.
  - 생성은 오브젝트 풀 매니저를 통해 이루어집니다.
- **NPC 종류**
  - Car와 Guest로 구성되어 있으며, 각각 해당 Spawner와 Manager가 관리합니다.

### 3.2. NPC Controller
![Waypoints](https://github.com/user-attachments/assets/bacc1852-58c0-4769-b015-ef2cf7205e34)

- **Waypoint 관리** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/60bad920ddef8afa78d04c82898a29378f8cdaea/Assets/Scripts/Manager/Game/WaypointManager.cs#L44)
  - 게임 시작 시 Waypoint 타입별 좌표값을 초기화합니다.
  - 설정된 Waypoint 타입에 따라 다음 이동 좌표를 제공합니다.

![Guest Controller](https://github.com/user-attachments/assets/52cf0b40-bc91-4305-915e-02d5ceb36406)

- **State 패턴** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/60bad920ddef8afa78d04c82898a29378f8cdaea/Assets/Scripts/Utils/State/CharacterState.cs#L235)
  - 캐릭터의 현재 상태를 관리합니다.
    - 상태 전환 시 실행되는 함수들을 관리합니다.
- **Guest 동작** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/60bad920ddef8afa78d04c82898a29378f8cdaea/Assets/Scripts/Character/GuestController.cs#L152)
  - 목표 Waypoint를 기준으로 이동을 설정하고 실행합니다.
    - 지정된 좌표에 도달하면 다음 좌표를 목표로 설정합니다.
  - 가게 입장 가능 여부를 판단하고 행동합니다.
    - 자연스러운 동선을 위해 확률 기반으로 가게 내부 이동을 결정합니다.
    - 픽업존과 웨이팅존이 모두 가득 찬 경우에는 내부 이동이 결정되어도 입장하지 않습니다.
  - 픽업존이 가득 찬 경우 웨이팅존으로 이동합니다.
  - 주문 후 인내시간 내에 음식을 수령하거나 실패하면 퇴장합니다.
    - 주문은 가능한 재료 중에서 무작위로 선택됩니다.

<figure class="half">  
  <img src="https://github.com/user-attachments/assets/89c20685-af82-4e2b-a20e-68fff5e1799f" alt="바퀴의 회전력으로 이동" width="350">
  <img src="https://github.com/user-attachments/assets/b0344baf-6fa7-4992-8220-2bad166bb3ba" alt="지면과 닿지 않으면 이동 불가" width="350"> 
</figure>

- **Car 동작** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/60bad920ddef8afa78d04c82898a29378f8cdaea/Assets/Scripts/Character/CarController.cs#L77)
  - Guest와 동일하게 목표 Waypoint를 기준으로 이동을 설정하고 실행합니다.
  - 자연스러운 이동을 위해 Wheel Collider를 사용했습니다.
    - 자동차 바퀴는 가속 힘에 비례하여 회전하며, 이 회전력으로 전진합니다.
  - 일정 거리 내에 다른 Car가 감지되면 제동력이 발생하여 정차합니다.

### 3.3. Player Controller

### 3.4. Food Controller

</div>
</details>

</br>

## 4. 핵심 트러블 슈팅
### 4.1. 예시 문제 이름
- 처음에 어떤 방식으로 구현했는지, 왜 그것을 사용했는지 이유를 적는다.

- 하지만 그로 인해 발생한 or 어떠한 이유로 인해 더 효율적인 것이 있다는 것을 알게 되어서 개선하려고 한다.

- 그런데 그렇게 되면 현재 구조와는 맞지 않기 때문에 구조 개선을 필요로 하게 된다.

<details>
<summary><b>기존 코드</b></summary>
<div markdown="1">

~~~c#
// example code
~~~

</div>
</details>

- 이 때 어떤 행동을 하면, 무슨 문제가 발생하기 때문에
- 아래 **개선된 코드** 와 같이 무엇을 사용해서 개선할 수 있었다.

<details>
<summary><b>개선된 코드</b></summary>
<div markdown="1">

~~~c#
// example code
~~~

</div>
</details>

</br>

## 5. 그 외 트러블 슈팅
<details>
<summary>예시 오류</summary>
<div markdown="1">

- 어떤 방식으로 해결함
- 참고 링크 이미지 등 첨부하면 좋을듯
- 오류 문구 첨부도 좋고
- 코드가 필요한 경우 추가

</div>
</details>

</br>

## 6. 회고 / 느낀점
> 필요하다면 추가
