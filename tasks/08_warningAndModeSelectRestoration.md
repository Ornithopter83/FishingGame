# 08. 주의사항 및 게임 모드 선택 Scene 복원

## 상태

- 완료

## 복원 수준 목표

- R3: 원본 Scene·코드·자산 근거에 따른 화면과 상태 전환 복원

## 목표

- 결제 완료 → LoadingScene → 주의사항 1 → 주의사항 2 → 게임 모드 선택 흐름을 복원한다.
- 두 주의사항은 원본과 같이 제한 시간이 지나거나 좌·우 어느 입력으로든 넘길 수 있게 한다.
- 모드 선택은 원본과 같이 SINGLE/BATTLE 선택, 선택 테두리와 비선택 차단막, 이동/선택 입력을 복원한다.
- 기존 `LastScene`의 임의 결과 데이터와 그 진입 경로를 제거한다.
- 직렬 장치가 없는 PC에서도 키보드 입력 어댑터로 같은 상태 전환을 확인할 수 있게 한다.

## 배경과 근거

### 흐름 근거

| 구간 | 원본 근거 | 확정 내용 |
|---|---|---|
| 결제 완료 → Loading | `ExportedProject/Assets/Scripts/InsertCardUi.cs` | 결제 완료 시 `FishGameManager.LoadScene = 2` 후 Loading으로 전환한다. |
| Loading → 선택 Scene | `ExportedProject/Assets/Scripts/DataManager.cs`, 원본 `LoadingSceneManager` DLL 역분석 기록 | 선택 Scene 번호는 2이며, 일반 게임 진입은 Addressables 배열의 `loadscene - 2` 항목을 연다. |
| 선택 Scene | `ExportedProject/Assets/776bde1206771494880c13770e84312e.unity` | `SelectSceneManager`, `Warning`, `ModeSelect`가 함께 존재하는 원본 Addressable Scene이다. |
| 주의사항 상태 | `ExportedProject/Assets/Scripts/SelectSceneManager.cs`, `State_Warning.cs` | 첫 화면 5초, 두 번째 화면 10초. 각 화면은 좌/우 어느 버튼으로도 즉시 넘긴다. 종료 후 `SELECTMODE` 상태로 간다. |
| 모드 선택 상태 | `ExportedProject/Assets/Scripts/State_PlayMode.cs`, `ModeSelectUi.cs` | 왼쪽 버튼은 SINGLE/BATTLE 선택을 토글하고 오른쪽 버튼은 선택을 확정한다. 초기 선택은 SINGLE이다. |

### 주의사항 화면 근거

- 첫 이미지: `Sprite/precautionsBg3.asset` + `Texture2D/precautionsBg3.png`
- 두 번째 이미지: `Sprite/rod_controller.asset` + `Texture2D/rod_controller.png`
- 첫 화면 문구와 위치는 원본 Scene YAML의 `Warning/Text`, `Text (1)`, `Text (2)`, 하단 안내 오브젝트를 따른다.
- 첫 화면의 주 경고 문구 알파는 원본 코루틴과 같이 0.4~0.8 사이를 반복한다.
- 두 번째 화면으로 바뀔 때 제목·주 경고·책임 안내를 숨기고 배경 이미지를 교체한다.

### 모드 선택 화면 근거

- 배경: `bg_gameDiffi _2`
- 상단 박스: `ui_diffiBox`
- SINGLE 카드: `singelMod`
- BATTLE 카드: `battleMod`
- 선택 테두리: `border_0`
- 입력 아이콘: `btn_l`, `btn_r`
- 원본 Scene 좌표: 카드 좌/우 x=±345, 선택 테두리 x=±364, 비선택 차단막 x=±362, y=-25.
- 상단 제목은 `게임 모드`, 하단 입력은 `이동`과 `선택`이다.

## 관련 파일/대상 자산

- `UnityProject/Assets/Scenes/Menu/SelectScene.unity`
- `UnityProject/Assets/Scripts/UI/SelectSceneController.cs`
- `UnityProject/Assets/Art/Selection/Warning/`
- `UnityProject/Assets/Art/Selection/Mode/`
- `UnityProject/Assets/Scripts/Core/SceneSession.cs`
- `UnityProject/Assets/Scripts/UI/TitleController.cs`
- `UnityProject/Assets/Scripts/Editor/SceneSetup.cs`

## 변경 금지

- `ExportedProject/`, `AuxiliaryFiles/` 직접 수정 금지
- 근거 없는 추가 문구, 장식, 버튼, 전환 연출 금지
- 다음 단계인 레벨 선택 화면을 임의로 생성하지 않는다.
- 네트워크 매칭 백엔드나 직렬 결제 장치 동작을 임의로 구현하지 않는다.

## 구현 단계

1. 원본 Addressable 선택 Scene과 관련 스크립트의 흐름·계층·좌표를 문서화한다.
2. 원본 이미지와 유효한 `.meta`를 정상 제작 경로에 채택한다.
3. 두 주의사항 화면과 5초/10초 자동 전환, 좌·우 입력 스킵을 복원한다.
4. SINGLE/BATTLE 모드 선택 화면과 이동/선택 상태를 복원한다.
5. Title의 PC 결제 대체 입력이 Loading을 거쳐 SelectScene으로 이어지도록 교체한다.
6. 임의 `LastScene` 결과 데이터와 Build Settings 진입 경로를 제거한다.
7. 정적 검증과 Unity PlayMode 수동 검증 결과를 분리해 기록한다.

## 완료 조건

- Title PC 입력 후 LoadingScene을 거쳐 SelectScene이 열린다.
- SelectScene 진입 시 첫 주의사항이 표시된다.
- 5초 또는 좌/우 입력 후 두 번째 주의사항이 표시된다.
- 10초 또는 좌/우 입력 후 게임 모드 선택 화면이 표시된다.
- 초기 선택이 SINGLE이며 이동 입력으로 선택 테두리와 차단막 위치가 교대한다.
- 기존 임의 물고기명·등급·중량 결과 UI와 `LastScene` 진입 경로가 남지 않는다.

## 자동 테스트

- Scene 생성기 정적 검증: 필수 자산, 컨트롤러, 계층, Build Settings 경로 확인
- Task 08 PlayMode 검증기: 두 경고 페이지 스킵, 모드 전환 및 선택 좌표 확인

## Unity 수동 테스트

1. Title에서 PC 결제 대체 입력(Enter/Space/클릭)을 누른다.
2. LoadingScene 뒤 첫 주의사항 이미지와 문구를 확인한다.
3. 좌/우 키 중 하나로 두 번째 이미지로 넘긴다.
4. 다시 좌/우 키 중 하나로 모드 선택 화면으로 넘긴다.
5. 이동 키로 SINGLE/BATTLE 선택 표시가 교대하는지 확인한다.
6. 선택 키 입력 시 현재 모드가 확정되는지 Console 기록을 확인한다.

## 원본 비교 방법

- `776bde1206771494880c13770e84312e.unity`의 Warning/ModeSelect 계층과 1920×1080 Game View를 비교한다.
- 원본 PNG 자체, RectTransform 좌표, 원본 코드의 시간·입력 전환을 각각 별도 근거로 대조한다.

## 위험과 되돌리기

- 위험: 원본 TMP/셰이더 효과 일부가 현재 프로젝트 패키지 구성에서 동일하게 렌더링되지 않을 수 있다.
- 되돌리기: SelectScene과 채택 자산, 관련 컨트롤러를 제거하고 SceneSession/Build Settings를 이전 상태로 복구한다.

## 결과

- 원본 해시형 Addressable Scene을 `SelectScene`으로 식별하고 결제 완료 뒤 실제 진입 대상을 확정했다.
- 원본 `precautionsBg3`, `rod_controller`, `ui_diffiBox`, `singelMod`, `battleMod`, `border_0` 자산과 유효한 `.meta`를 정상 제작 경로에 채택했다.
- 주의사항 1(5초) → 주의사항 2(10초) → 모드 선택 전환과 양쪽 입력 스킵을 구현했다.
- SINGLE 초기 선택과 BATTLE 토글 시 선택 테두리 x=-364/364, 차단막 x=362/-362 교대를 구현했다.
- Title의 PC 결제 대체 입력을 `LoadingScene -> SelectScene`으로 교체했다.
- 임의 `LastScene`, `ResultController`, Task 06 임의 결과 흐름 검증기, Build Settings 항목을 제거했다.
- Unity Scene/import/compile 검증 통과: `UnityProject/Logs/Task08SceneCreateFinal.log`
- Unity PlayMode 전체 흐름 및 입력 검증 통과: `UnityProject/Logs/Task08PlayModeVisualGpu.log`
- 실제 GPU 1920×1080 캡처 3종을 `UnityProject/Logs/Task08WarningPage1.png`, `Task08WarningPage2.png`, `Task08ModeSelect.png`로 확인했다.
- Unity 빌드, 스테이징, 커밋, 푸시는 수행하지 않았다.

## 잔여 불확실성

- SINGLE 확정 뒤의 레벨 선택은 다음 Scene 복원 범위이며 이번 작업에서는 생성하지 않는다.
- BATTLE 확정 뒤의 네트워크 매칭 UI와 백엔드는 원본에 존재하지만, 서버 및 하드웨어 경계 검증 전에는 활성 복원하지 않는다.
- 원본 TextMeshPro 재질의 세부 외곽선 효과는 현재 SCDream5 폰트 기반 UI Text 어댑터와 시각 차이가 날 수 있다.
