# 06. Scene 시각·기능 복원

## 상태

- 재작업 필요: 기존 `Loading`, `LastScene`, `Title` 화면은 원본 근거가 부족한 임의 구성이 포함되어 완료 판정을 취소한다.
- 현재 인정 범위: PC Scene 전환 scaffold와 일부 Title 원본 오브젝트 조사
- 시각 복원 수준: 미확정. 원본 비교가 끝날 때까지 R3로 표시하지 않는다.
- 최종 갱신: 2026-07-16
- 빌드·커밋·푸시: 수행하지 않음

## 목표

- 05번에서 분류한 Scene을 실제 화면 근거와 원본 YAML/GUID에 따라 복원한다.
- Serial, 카드 단말기 및 모터가 없는 PC에서도 흐름이 중단되지 않게 한다.
- 원본과 동일한 부분과 보수적으로 재구성한 부분을 구분해 남긴다.
- 원본 근거 없는 화면 구성은 일체 금지한다. 근거가 없으면 구현하지 않는다.

## 구현 금지

- 원본에 없거나 확인되지 않은 UI, 버튼, 배경, 캐릭터, 결과 항목, 장식 및 전환 연출
- 단순히 Scene 흐름을 보여 주기 위한 임의 화면
- 사용자 승인 없는 `Estimated`/`Placeholder`의 시각 배치
- 전체 참조 이미지를 런타임 배경으로 사용해 복원을 대신하는 방식

## 근거

- 원본 Scene: `ExportedProject/Assets/Scenes/Title.unity`
- 원본 빌드 순서: Title(0), Loading(1), LastScene(2)
- 사용자 제공 실행 화면: `Docs/ReferenceImages/TitleScreen_1920x1080.png`
- 참조 이미지 SHA-256: `2E3436FAEDF569FB80C0A0F02F0618F41C7B7AEDDE2085BBFCB46DDD44D04FD2`
- 원본 코드 근거: `State_Title`, `InsertCardUi`, `SerialManager`, `LoadingSceneManager`
- Title 직접 GUID 370개 중 368개가 export에 존재하며 2개는 Unity built-in이다. 전체 전이 의존성은 1,182개, 약 474.38 MiB이다.

## 06-1 Loading

- 기존 화면은 사용자 확인 결과 원본과 다르므로 시각 복원 완료 판정을 취소한다.
- 원본 hierarchy와 RectTransform, 실제 실행 이미지가 다시 확정될 때까지 임의 progress/animation을 추가하지 않는다.
- PC 모드의 하드웨어 비대기 전환은 기능 scaffold로만 유지한다.

## 06-2 LastScene

- 기존 화면은 사용자 확인 결과 원본과 다르므로 시각 복원 완료 판정을 취소한다.
- 샘플 결과와 임의 UI는 원본 복원의 기준으로 사용하지 않는다.
- 원본 hierarchy, 자산, 실행 화면을 다시 조사한 뒤 확정된 요소만 복원한다.

## 06-3 Title 재복원

### 정적 이미지가 아닌 실제 구성

- 참조 화면 전체를 Sprite로 붙이는 이전 구현은 제거했다.
- Scene에는 다음이 서로 분리된 실제 오브젝트로 존재한다.
  - 3D: `Gwangan Bridge`, `ocean`, `Ships`, `yacht`, `yacht (1)`, `Cruiseship_RotatingRadars`
  - UI: `titleLogo`, `symbol`, `Card/payment_text`, `battery`, `power_lv`, `clinet_version`
  - 동작: `TitleController`, `TitleStatusView`, 각 선박의 `TitleShipMotion`
- `TitleReferenceScreen`, 임의 PC 메뉴, Character preview, 임의 Start/Result/Loading 버튼은 Scene 검증에서 금지한다.
- 사용자 제공 이미지는 `Docs/ReferenceImages`의 비교 증거로만 보관하며 런타임 참조가 없다.

### Original

- Main Camera 위치 `(0, 5, 0)`, 회전, FOV 60, near 0.3, far 500
- 광안대교 Mesh, 6개 원본 재질용 Albedo Texture, Transform
- 중앙 Strong Fisher 로고, 좌상단 Fishing Family 로고, 배터리 Sprite와 UI RectTransform
- 요트 2척과 크루즈선 Mesh/Texture, 최초 Transform 및 원본 SWS 경로 좌표
- Title BGM `bgm_title.ogg`
- 최초 결제 안내 문구: `카드 단말기에 카드를 꽂거나 터치하게 되면 결제 후 게임이 시작 됩니다.`
- 화면 근거의 장력 `3`, 버전 `L 1.3.5`, 적색 배터리 상태

### Reconstructed

- `TitleStatusView`는 결제 안내, 장력, 버전, 배터리를 개별 참조로 보유한다.
- `SetPaymentMessage`, `SetTensionLevel`, `SetClientVersion`, `SetBatteryState`로 런타임 변경할 수 있다.
- PC 확인 키: 위/아래 방향키는 장력, B는 배터리 상태를 변경한다.
- `TitleShipMotion`은 누락된 SWS `splineMove` 대신 원본 경로 좌표를 따라 실제 Transform을 이동·회전시킨다.
- Enter, Space 또는 좌클릭은 카드/우측 버튼 입력의 PC adapter로 동작해 Loading을 거쳐 LastScene으로 간다.
- `FishingGame/Verify Task 06 PC Flow`는 UI 값 변경, 선박 이동 및 전체 Scene 루프를 자동 검증한다.

### Estimated / Placeholder

- Enviro 3의 정확한 버전과 라이선스가 없어 현재 하늘은 Unity Procedural Sky 대체이다. 구름이 없는 상태이며 `Estimated`이다.
- Crest 원본 패키지 대신 원본 수면 Texture를 사용하는 `FishingGame/TitleWater` 셰이더를 제작했다. 파형, 반사색, 포말 강도는 제공 화면에 맞춘 `Estimated` 값이다.
- 원본 bridge shader export가 사용 불가능한 dummy이고 원본 URP/전용 shader가 없어서, bridge는 원본 Albedo를 사용하는 보수적 Built-in Standard 대체 재질이다. Mesh, submesh 순서와 Transform은 원본이다.
- 정확한 날씨 애니메이션, 수면 시뮬레이션 및 SWS 보간 timing은 패키지 확인 전까지 복구 완료로 간주하지 않는다.

### 누락했던 원본 유휴 루프

- 원본 `GameSupport`에서 `30초 대기 -> 랭킹 -> 안내 이미지 2장 -> 영상 -> Title 복귀` 반복 흐름을 확인했다.
- 원본 영상은 `fishing.webm`이며 실행 폴더의 `Intro/Intro.mp4`와 `IntroSound.mp3`를 우선 사용할 수 있다.
- 현재 export의 `TitlteRankUi`와 `DataManager`는 30개 랭킹을 사용하며 사용자 확인도 30개로 확정되었다.
- 상세 근거: `Docs/TITLE_IDLE_LOOP_ANALYSIS.md`
- `TitleIdleLoop`에 원본 루틴을 복원했다.
  - 정상 대기 30초
  - 30개 랭킹과 `0 -> 765 -> 1545 -> 2120 -> 0` 스크롤
  - 3초 구간 대기, 1초 이동, 복귀 후 1.5초 대기
  - 안내 이미지 8개 중 2개를 중복 없이 각 3.5초 표시
  - Loading BGM 전환 후 `fishing.webm` 또는 실행 폴더의 `Intro/Intro.mp4` 재생
  - 영상 fade out 후 Title BGM과 Title 화면으로 복귀하고 반복
- 랭킹은 원본 PlayerPrefs 키 `nRank`, `sRank`, `cRank`, `dRank`의 1~30번을 읽으며, 데이터가 없을 때 원본 기본 생성 규칙을 사용한다.
- 자동 검증 전용 단축 시간은 검증 호출 시에만 사용하며 실제 실행의 원본 시간 값을 변경하지 않는다.

### 수면 재작업

- 원본 Crest 설정은 wind speed 150, wave heading 0, turbulence 0.145, horizontal displacement 15 및 foam simulation을 사용한다.
- 사용자가 확인한 우측 방향 요동 효과를 기준으로 작은 단계로 추가하고 매 단계 화면 비교를 받는다.
- `TitleWater`에 +X 방향 파동 진행, 수평 chop, +X normal scroll을 1차 반영했다.
- Crest FFT와 foam을 동일하게 재현한 것은 아니므로 `Estimated` 상태이며 단계별 화면 비교가 남아 있다.

## 채택 자산

- `Assets/Models/Title/Mesh`: 광안대교, 요트, 크루즈 Mesh
- `Assets/Materials/Title`: 원본 Material 증거와 생성된 호환 Material
- `Assets/Art/Title/Textures`: 교량, 선박, 수면, 로고, 배터리 Texture
- `Assets/Art/Title/Sprites`: 원본 Sprite asset
- `Assets/Art/Fonts/SCDream5.otf`
- `Assets/Audio/BGM/bgm_title.ogg`, `bgm_loading.ogg`, `Cave_02.ogg`
- `Assets/Art/Title/Idle`: 랭킹/안내 Sprite, 의존 Texture, intro RenderTexture
- `Assets/Video/Title/fishing.webm`
- `Assets/Audio/Title/Intro_bg.ogg`
- 선택한 원본 자산은 유효한 `.meta`를 함께 복사해 GUID를 보존했다.

## 검증 결과

- 정적/컴파일/Scene 검증: 통과
  - 로그: `UnityProject/Logs/TitleIdleLayoutFix.log`
  - 결과: `Scene validation passed: Bootstrap, Loading, LastScene, Title.`
- 실제 GPU 1920×1080 렌더 캡처: 완료
  - 결과: `UnityProject/Logs/TitleObjectRender.png`
  - 랭킹 결과: `UnityProject/Logs/TitleRankRender.png`
  - 로그: `UnityProject/Logs/TitleIdleRankGpuFinal.log`
- Play Mode 자동 검증: 통과
  - 로그: `UnityProject/Logs/TitleIdleLoopPlayModeFinal.log`
  - 결제 문구와 장력 값의 런타임 변경 확인
  - 선박 Transform 이동 확인
  - 단축 검증 시간으로 랭킹, 안내 이미지 2장, 영상, Title 복귀 1회 확인
  - `Title -> Loading -> LastScene -> Loading -> Title` 확인
- Unity 버전: `2022.3.62f3`
- Unity 빌드: 미수행
- 실제 Serial/카드/모터 장치: 미검증

## 남은 불확실성

- Enviro 3, Crest, SWS의 정확한 원본 버전과 라이선스
- 원본 구름·수면·선박 animation timing
- 결제/카드 입력 발생 시 실행 중인 유휴 루프 취소와 GAMEWAIT 전환의 정확한 원본 우선순위
- 정상 30초 대기 및 외부 `Intro/Intro.mp4`를 포함한 장시간 수동 실행 결과
- 결제 성공부터 장력 선택 완료까지의 실제 cabinet 조작 영상
- 배터리 및 장력 값이 실제 하드웨어와 갱신되는 adapter 구현

## 결과

- Title은 더 이상 전체 화면 이미지에 의존하지 않는다.
- 하단 결제 문구는 변경 가능한 Text이며, 선박은 독립 GameObject와 이동 컴포넌트를 가진다.
- Title 유휴 루프와 우측 방향 수면의 1차 동작은 복원했지만, 수면은 `Estimated`이고 Loading/LastScene도 사용자 확인상 원본과 다르다.
- 따라서 Task 06은 시각 복원 완료가 아니라 재조사·재작업 상태다.
