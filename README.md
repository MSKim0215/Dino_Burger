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
![Base](https://github.com/user-attachments/assets/e106a068-4135-4a8f-995b-db6d5eed7c4a)
- 본 시스템은 MVC 아키텍쳐를 기반으로 하여, Controller가 Data와 View 계층을 효율적으로 관리합니다.
- Controller는 Data 계층으로부터 수신한 정보를 분석하고 처리하여 시스템의 핵심 로직을 수행합니다.
- View 계층은 Controller의 지시에 따라 사용자 인터페이스를 갱신하며, Controller로부터 전달받은 데이터를 시각적으로 표현합니다.

</br>

- **NPC 시스템**
  - 체계적인 NPC 생성 및 관리를 위한 Spawner 시스템을 구현했습니다.
  - 효율적인 동선 관리를 위해 Waypoint 시스템과 상태 머신을 도입하여 NPC의 행동을 최적화했습니다.
- **Player 시스템**
  - 정밀한 물체 감지 및 상호작용을 위한 Raycast 시스템을 구현했습니다.
  - 직관적인 물체 조작을 위해 Hand 시스템을 도입하고, 다양한 오브젝트와의 유기적인 상호작용 메커니즘을 구현했습니다.
- **Food 시스템**
  - 최적화된 리소스 관리를 위해 오브젝트 풀링 시스템을 적용하여 효율적인 자원 활용을 하도록 구현했습니다.

</br>

<details>
<summary><b>핵심 기능 설명 펼치기</b></summary>
<div markdown="1">

</br>

### 3.1. NPC 시스템
<img src="https://github.com/user-attachments/assets/82fcc4a3-c74e-4ab7-a98f-a13cc6fe5e9b" width="700">
<details>
  <summary><b>클래스 구조도</b></summary>
  <div markdown="2">
    <img src="https://github.com/user-attachments/assets/48a913c4-0c12-4b61-89e0-12c53683303f">
  </div>
</details>

- **초기 환경 구성** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/26f141d32664c3031c122082ff2f87f32028f7fd/Assets/Scripts/Manager/Game/GuestManager.cs#L15)
  - 시스템 시작 시 사전 정의된 생성 위치를 설정하여 초기화를 수행합니다.
- **캐릭터 생성 프로세스** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/26f141d32664c3031c122082ff2f87f32028f7fd/Assets/Scripts/Manager/Game/GuestManager.cs#L142)
  - 최적화된 오브젝트 풀 시스템을 활용하여 지정된 위치에서 NPC를 주기적으로 생성합니다.
- **캐릭터 분류 체계**
  - 차량(Car)과 손님(Guest) 두 종류로 구분되며, 각각 전용 Spawner와 Manager를 통해 체계적으로 관리됩니다.

</br>

<img src="https://github.com/user-attachments/assets/0f416d26-b610-4304-aacd-100c78a1e01e" width="700">
<details>
  <summary><b>클래스 구조도</b></summary>
  <div markdown="2">
    <img src="https://github.com/user-attachments/assets/bacc1852-58c0-4769-b015-ef2cf7205e34">
  </div>
</details>

- **이동 경로 시스템** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/60bad920ddef8afa78d04c82898a29378f8cdaea/Assets/Scripts/Manager/Game/WaypointManager.cs#L44)
  - 시스템 초기화 시 객체 유형별 이동 좌표를 설정하고 경로 데이터를 관리합니다.

</br>

<img src="https://github.com/user-attachments/assets/7955241c-3589-4f06-afac-d74e5da4beca" width="350">
<img src="https://github.com/user-attachments/assets/99904970-5576-4cfb-8adf-12e7ecb4bde5" width="350">

<details>
  <summary><b>클래스 구조도</b></summary>
  <div markdown="2">
    <img src="https://github.com/user-attachments/assets/52cf0b40-bc91-4305-915e-02d5ceb36406">
  </div>
</details>

- **상태 관리 패턴** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/60bad920ddef8afa78d04c82898a29378f8cdaea/Assets/Scripts/Utils/State/CharacterState.cs#L235)
  - 체계적으로 NPC 상태를 전환하고 행동 로직을 실행합니다.


<img src="https://github.com/user-attachments/assets/227c0355-fe6c-4464-8682-eb7dccd6b32a" width="350">
<img src="https://github.com/user-attachments/assets/0a710202-76d4-4751-a7bc-01df65a13569" width="350">

- **캐릭터 행동 프로세스** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/60bad920ddef8afa78d04c82898a29378f8cdaea/Assets/Scripts/Character/GuestController.cs#L152)
  - 목표로 지정된 좌표를 향해 이동을 수행합니다.
    - 해당 좌표에 도달하면 다음 순서의 좌표를 새로운 목표 지점으로 설정합니다.
  - 웨이팅존과 픽업존의 수용 한도 초과 시 추가 입장이 제한됩니다.
  - 픽업존이 만석일 경우, 자동으로 웨이팅존으로 경로가 재설정됩니다.
  - 제한시간 내에 음식을 수령하거나 받지 못하면 가게를 떠납니다.

</br>

<img src="https://github.com/user-attachments/assets/89c20685-af82-4e2b-a20e-68fff5e1799f" width="350">
<img src="https://github.com/user-attachments/assets/b0344baf-6fa7-4992-8220-2bad166bb3ba" width="350"> 

- **Car 동작** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/60bad920ddef8afa78d04c82898a29378f8cdaea/Assets/Scripts/Character/CarController.cs#L77)
  - Wheel Collider의 가속력으로 바퀴 회전을 하여 자연스러운 주행이 가능합니다.
  - 전방 차량을 Raycast로 감지하여 정차합니다.

</br>
  
### 3.2. Player 시스템
<img src="https://github.com/user-attachments/assets/040c59df-bf49-4cc9-853f-6dc5bf18071a" width="350">
<details>
  <summary><b>클래스 구조도</b></summary>
  <div markdown="2">
    <img src="https://github.com/user-attachments/assets/8be398ee-8647-49f3-988f-698c20bb9057">
  </div>
</details>

- **물체 인지 메커니즘** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/be5cbcaedd21fb791f62fd10d971912d028e8fe8/Assets/Scripts/Character/PlayerController.cs#L143)
  - 전방 부채꼴 형태의 Raycast를 통해 객체를 검출하며, 가장 높은 빈도로 감지되는 객체를 우선 선별합니다.
    - Table 유형 객체는 시각적 피드백이 제공되며, Wall/Table 감지 시 이동이 자동 제한됩니다.
   
</br>

<img src="https://github.com/user-attachments/assets/a1784f56-45d6-44eb-bebf-0ba479da4d26" width="350">
<img src="https://github.com/user-attachments/assets/9132d73c-eabf-481b-be22-e849929a8452" width="350">

- **객체 상호작용** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/be5cbcaedd21fb791f62fd10d971912d028e8fe8/Assets/Scripts/Utils/Hand.cs#L5)
  - 모든 상호작용 대상 객체는 Hand 컴포넌트가 구현되어 있습니다.
  - HandObject가 null이 아닌 객체들 간 상호 교환이 가능하도록 설계되었습니다.

</br>

<img src="https://github.com/user-attachments/assets/64cd1c74-7bd2-4e5a-bfd2-84bc3507d130" width="233">
<img src="https://github.com/user-attachments/assets/50ef5dab-39f6-4deb-9737-1e75d861d1ac" width="233">
<img src="https://github.com/user-attachments/assets/6fbb5a41-6c40-42a4-ad3a-fbb02d22fb6f" width="233">

- **기능별 상호작용 체계** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/be5cbcaedd21fb791f62fd10d971912d028e8fe8/Assets/Scripts/HandNotAble/Table/CuttingBoardTableController.cs#L89)
  - Table 및 Crate(재료 보관함)와의 상호작용이 구현되어 있습니다.
  - 조리대에서는 지정된 재료만 가공이 허용됩니다.
  - 재료 보관함은 미가공 상태의 재료만 수납이 가능합니다.

</br>

### 3.3. Food 시스템
<img src="https://github.com/user-attachments/assets/9718edbf-7bbc-4812-a009-34dba6ef056a" width="350">
<details>
  <summary><b>클래스 구조도</b></summary>
  <div markdown="2">
    <img src="https://github.com/user-attachments/assets/8638fa4a-49a3-40c1-a4e8-8eae4d7aa0f5">
  </div>
</details>

- **햄버거빵 가공 프로세스** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/be5cbcaedd21fb791f62fd10d971912d028e8fe8/Assets/Scripts/HandAble/BunIncredientController.cs#L8)
  - 재료 조합 시 햄버거로의 자동 전환이 이루어집니다.
  - 효율적인 오브젝트 풀링을 통해 음식 생성을 최적화합니다.
 
</br>

<img src="https://github.com/user-attachments/assets/8d0b65cd-3bf7-4ef6-8f42-e797633293ca" width="350">
<details>
  <summary><b>클래스 구조도</b></summary>
  <div markdown="2">
    <img src="https://github.com/user-attachments/assets/2eea027e-8e16-4525-bb9a-bfaef5251905">
  </div>
</details>

- **햄버거 스택 시스템** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/be5cbcaedd21fb791f62fd10d971912d028e8fe8/Assets/Scripts/HandAble/Food/BurgerFoodController.cs#L51)
  - 재료가 규격화된 모델링 기준에 따라 체계적으로 적층됩니다.
  - 재료 구성 정보를 실시간 인터페이스로 시각화합니다.

</br>

<img src="https://github.com/user-attachments/assets/400813ed-5243-48d1-ae39-558d8b15bc1b" width="350">
<details>
  <summary><b>클래스 구조도</b></summary>
  <div markdown="2">
    <img src="https://github.com/user-attachments/assets/49b5b6f5-52fb-48cc-9804-9314619ddef7">
  </div>
</details>

- **스튜 제작 시스템** 📌 [코드 확인](https://github.com/MSKim0215/Dino_Burger/blob/be5cbcaedd21fb791f62fd10d971912d028e8fe8/Assets/Scripts/HandNotAble/Table/PotTableController.cs#L59)
  - 지정된 재료 투입 시점부터 조리 공정이 개시됩니다.
  - 재고 소진 시 자동으로 기본 상태로 초기화됩니다.

</div>
</details>
