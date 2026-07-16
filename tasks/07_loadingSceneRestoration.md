# 07. LoadingScene 원본 복원

## 상태

- 구현 완료, 사용자 실행 화면 기반 밝기 보정 완료

## 복원 수준 목표

- R4: 원본 Scene 계층, 원본 자산, 원본 Spine 애니메이션과 PC 전환 기능 복원

## 목표

- 임의 제작된 Loading UI를 전부 제거한다.
- 원본 `LoadingScene.unity`의 배경과 중앙 Spine 로딩 애니메이션을 복원한다.
- 하드웨어가 없는 PC 모드에서도 다음 Scene으로 전환되는 기존 adapter 경계를 유지한다.

## 배경과 근거

- DLL 근거: `ExportedProject/Assets/Plugins/spine-unity.dll`, `spine-csharp.dll`
- Scene 근거: `ExportedProject/Assets/Scenes/LoadingScene.unity`
- 에셋 근거: `loading_0.json`, `loading.atlas_0.txt`, `loading_0.png`, `loading_SkeletonData_0.asset`, `loading_Atlas_0.asset`
- 코드 근거: `ExportedProject/Assets/Scripts/LoadingSceneManager.cs`
- 사용자 확인: 물고기가 화면 중앙에서 뱅글뱅글 도는 Loading 화면
- 사용자 제공 실행 이미지(2026-07-16): `LOADING...` 문자는 배경 때문에 어두워지지 않고 밝은 흰색으로 명확하게 표시된다.
- 화면 배치 추정 사항: 없음. 확보된 Scene 직렬화 값만 사용한다.
- Shader 예외: Export 결과의 `Spine_SkeletonGraphic.shader` 본문이 `DummyShaderTextExporter`로 소실되어, 원본 재질 GUID와 straight-alpha/PMA mesh 계약을 유지하는 투명 UI pass를 `Reconstructed`로 복구한다.

## 관련 파일/타입/에셋

- 원본 hierarchy: `Canvas/back/subBack`, `SkeletonGraphic (loding)`, `loadingManager`
- 배경 Sprite: `bg_gameDiffi _2`
- Spine animation: `loding_x2`, loop=true, timeScale=2
- SkeletonGraphic RectTransform: `586.10004 x 633.6256`, 중앙 배치
- 회전 근거: `bone2`가 5초 동안 약 359.5도 회전하며 timeScale 2로 재생
- 합성 근거: 원본 SkeletonGraphic의 `pmaVertexColors: 1`과 사용자 실행 이미지의 밝은 흰색 문자를 함께 만족하도록 PMA blend를 사용

## 변경 금지

- 원본 보존 영역 직접 수정 금지
- 관계없는 Scene/Prefab 저장 금지
- 근거 없는 로고, 카드, 진행 막대, 안내 문구 추가 금지
- 원본 Spine 자산을 임의 Sprite 회전으로 대체 금지

## 구현 단계

1. 임의 `CinematicTint`, `FishingFamilyLogo`, `LoadingCard`, 진행 막대, PC 안내 제거
2. 원본 Spine 런타임 DLL과 Loading 의존 자산을 정상 제작 경로로 채택
3. 원본 hierarchy와 직렬화 값으로 Loading Scene 재생성
4. PC 전환 controller 연결
5. 정적/Play Mode/GPU 검증
6. `CURRENT_STATE.md` 갱신

## 완료 조건

- Loading Scene에 임의 UI가 남지 않는다.
- 원본 해변 배경과 중앙 `SkeletonGraphic (loding)`만 표시된다.
- `loding_x2`가 loop=true, timeScale=2로 재생된다.
- PC 전환 흐름이 중단되지 않는다.

## 자동 테스트

- Scene에 금지된 임의 오브젝트가 없는지 검사
- `SkeletonGraphic`의 SkeletonData, animation name, loop, timeScale 검사
- LoadingController 및 Build Settings 검사

## Unity 수동 테스트

1. Loading Scene을 열어 원본 배경과 중앙 물고기 애니메이션을 확인한다.
2. 물고기와 원형 효과가 반복 회전하는지 확인한다.
3. Title에서 진입 후 LastScene 또는 Title로 정상 전환되는지 확인한다.

## 원본 비교 방법

- 원본 Scene YAML의 hierarchy, RectTransform, 색상, 애니메이션 직렬화 값을 비교한다.
- 1920x1080 GPU 캡처로 중앙 배치와 배경을 비교한다.

## 위험과 되돌리기

- 위험: 조각 자료의 Spine DLL이 Unity 2022.3에서 호환되지 않을 수 있다.
- 되돌리기: Loading 채택 자산과 SceneSetup의 Loading 구간만 제거할 수 있도록 경로를 분리한다.

## 결과

- 임의 `OriginalBackground`, `CinematicTint`, `FishingFamilyLogo`, `LoadingCard`, 상태 문구, 점 애니메이션, 진행 막대와 PC 안내를 제거했다.
- 원본 해변 배경 `bg_gameDiffi _2`와 원본 Spine 데이터·Atlas·Texture·Material을 정상 제작 경로 `Assets/Art/Loading`에 채택했다.
- 원본 `spine-unity.dll`, `spine-csharp.dll`을 `Assets/Plugins/Spine`에 채택하고 원본 `.meta` GUID를 보존했다.
- `Canvas/back/subBack/SkeletonGraphic (loding)` 계층, Canvas shader channel 25, 배경색, 중앙 크기·pivot, `loding_x2`, loop=true, timeScale=2를 원본 직렬화 값으로 복원했다.
- Export에서 본문이 소실된 SkeletonGraphic shader의 투명 UI 렌더 pass를 `Reconstructed`로 복구했다. 원본 `pmaVertexColors: 1`에 맞춰 straight-alpha Atlas를 fragment에서 premultiply하고 `Blend One OneMinusSrcAlpha`로 합성한다. 화면 디자인 값이나 신규 자산은 추가하지 않았다.
- 사용자 제공 실행 이미지와 비교해 이중 alpha 곱으로 어두워졌던 `LOADING...` 문자를 밝은 흰색으로 교정했으며, 투명 픽셀 RGB가 번지는 현상도 제거했다.
- `LoadingController`는 원본처럼 비동기 로드 진행률 0.89 도달 후 2초간 화면을 유지하고 scene activation을 허용한다.
- 정적 검증 통과: `UnityProject/Logs/Task07StaticValidationFinal.log`
- Play Mode 검증 통과: `UnityProject/Logs/Task07LoadingPlayModeFinal.log`
  - `loding_x2` track 활성화와 `bone2` 회전값 진행 확인
  - 1920×1080 GPU 캡처: `UnityProject/Logs/LoadingOriginalRender.png`
- 사용자 화면 기반 PMA 밝기 교정 검증 통과: `UnityProject/Logs/Task07LoadingPmaCorrection.log`
- 전체 PC 흐름 검증 통과: `UnityProject/Logs/Task07FullSceneFlow.log`
  - `Title -> Loading -> LastScene -> Loading -> Title`
- Unity 빌드, 스테이징, 커밋, 푸시는 수행하지 않았다.

## 남은 불확실성

- 원본 Serial 모드의 장치 연결 대기 동작과 Addressables backend는 장치·원본 서비스가 없어 미검증이다.
- Export에서 유실된 원본 SkeletonGraphic shader 소스와의 픽셀 단위 동일성은 보장할 수 없다. 현재 캡처는 원본 Texture와 애니메이션을 straight-alpha로 표시한 복원 결과다.
- 원본 Loading BGM 호출은 전역 `SoundManager` 복원 범위이므로 이번 Scene에 임의 AudioSource를 추가하지 않았다.
