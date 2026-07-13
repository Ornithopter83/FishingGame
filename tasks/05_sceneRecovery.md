# 05. Scene 단위 복원

## 상태

- 기반 준비 완료, Scene별 후속 복원 진행 예정

## 목표

- 복구 작업을 Scene 단위로 순차 진행한다.
- 각 Scene에 필요한 코드, 패키지, Prefab, Material, Animator, Audio, Data만 함께 가져온다.
- 과도하게 잘게 나누지 않고 이 문서의 Scene 표에서 상태와 결과를 관리한다.

## 공통 절차

1. 원본 Scene YAML의 GUID와 custom component 목록을 작성한다.
2. 직접 및 간접 의존 파일 manifest를 만든다.
3. Unity 기본/UPM/Asset Store/DLL/native plugin 경계를 분류한다.
4. 채택할 Scene과 유효한 `.meta`를 정상 `Assets/Scenes/...` 경로에 배치한다.
5. 필요한 Script, Data, Prefab, Material, Animator, Audio도 정상 제작 경로에 배치한다.
6. 임시 비교나 변환 복사본만 `_ReferenceTemp`에 두고 제품 참조를 만들지 않는다.
7. Missing Script/Reference와 compile/import 오류를 기록하고 해소한다.
8. 하드웨어·결제·네트워크는 simulator/adapter로 대체한다.
9. Unity 수동 실행과 화면/전환을 검증하고 `CURRENT_STATE.md`를 갱신한다.

## Scene 복원 목록

| 순서 | 논리 이름 | 원본 Scene | 역할 | 현재 상태 |
|---:|---|---|---|---|
| 0 | Bootstrap/Test | 신규 | 검증 진입점, adapter, 로그 | R2 기본 Scene 생성/검증 |
| 1 | Loading | `Scenes/LoadingScene.unity` | Scene 전환, loading UI | R2 안전 shell; 원본 layout 대기 |
| 2 | Busan Gameplay | `e661fe7e48f559a40970741e68ca9fc8.unity` | 첫 Gameplay 수직 조각 기준 map | 구조 확인 |
| 3 | Result/Last | `Scenes/LastScene.unity` | 결과 UI, Fish card | 구조 확인 |
| 4 | Title | `Scenes/Title.unity` | 진입, 설정, 선택, 시스템 manager | 구조 확인; 결합도 높음 |
| 5 | Select Player/Character | `776bde1206771494880c13770e84312e.unity` | `LoadScene=2` 선택 화면 (`Estimated`) | 역할 추가 확인 필요 |
| 6 | Incheon Gameplay | `926600b55d7004b4ba4420d4ebc2aba6.unity` | Gameplay map | 공통 기반 대기 |
| 7 | Pohang Gameplay | `4a90dea7b55b6844dab8e1f52ac402e1.unity` | Gameplay map | 공통 기반 대기 |
| 8 | Osaka Gameplay | `9c14a515c7cd3a341ac3b298d80a115d.unity` | Gameplay map | 공통 기반 대기 |
| 9 | Sydney Gameplay | `49199a014f95e754680a7bc67316c4e8.unity` | Gameplay map | 공통 기반 대기 |
| 10 | Mekong Gameplay | `9336adcd49129d54a8b218f89c9ddfd8.unity` | Gameplay map | 공통 기반 대기 |
| 11 | Amazon Gameplay | `54544c6a5cc50d74b9cb7928f02a336c.unity` | Gameplay map | 공통 기반 대기 |
| 12 | Baikal Gameplay | `7a6d424450b49d44385a1562124f93da.unity` | Gameplay map | 공통 기반 대기 |
| 13 | Antarctica Gameplay | `1860b791fe332d5469ef6bfe7a064ecd.unity` | Gameplay map | 공통 기반 대기 |
| 14 | USA Gameplay | `41cba86cf8bae97448ca454ab3c6ec46.unity` | Gameplay map | 공통 기반 대기 |
| 15 | Mystic Gameplay | `427872b136b426d43abbb4b07f3dc390.unity` | 최종 Stage, `stageNumber=98` | 공통 기반 대기 |

복원 순서는 의존성이 작은 기반부터 시작하며, 최종 실행 흐름은 `Title → Loading → Select/Gameplay → Last`로 연결한다.

## Scene 0: Bootstrap/Test

- [x] `Assets/Scenes/Bootstrap/Bootstrap.unity` 생성
- [x] `BootstrapController`, `SceneSession`, `GameLog` 구현
- [x] 하드웨어와 외부 서비스 없이 실행 가능한 진입점 구성
- [x] Build Settings 등록 및 batch compile/component 검증
- [x] Unity Editor에서 Bootstrap Scene 저장·재로드 가능
- [ ] Bootstrap → Loading → Bootstrap Play Mode 수동 확인

## Scene 1: Loading

### 조사 결과

- 직접 GUID 13개: 원본 자료 11개, Unity built-in 2개, 원인 불명 missing 0개
- custom script는 `LoadingSceneManager` 1개
- runtime에서 Sound, Serial, FishGame, State, Ads, Addressables와 직접 결합
- Serial 모드에서는 장치 연결까지 무기한 대기할 가능성이 있음

### 현재 구현과 남은 조건

- [x] `Assets/Scenes/Bootstrap/LoadingScene.unity` 안전 shell 생성
- [x] `LoadingController`가 Serial/Addressables 없이 명시적인 target Scene을 로드하도록 구현
- [x] Unity Editor에서 Loading Scene 저장·재로드 가능
- [ ] 직접/간접 asset closure와 package 경계 확정
- [ ] 원본 layout에 필요한 자산을 정상 경로로 가져오기
- [ ] Loading layout 표시
- [ ] Bootstrap → Loading 전환 수동 확인
- [ ] 원본 flow 연결 상태 기록

## Scene 2: Busan Gameplay

첫 Gameplay 기준 Scene으로 공통 manager, 낚싯대, 장력 UI, 물고기, 상태 흐름의 최소 수직 조각을 복원한다. 이후 Gameplay map은 이 공통 기반을 재사용한다.

- [ ] Scene GUID/component/dependency manifest
- [ ] `StageData_stage_busan`과 map data 연결
- [ ] `StageManager`, `MainObjectCont`, `RodCont`, `TensionCont` 참조 해소
- [ ] Casting → Wait → Bite → Hooking → Fight/Holding → Finish/Catch
- [ ] keyboard/mouse simulator 동작
- [ ] Result Scene 전환 또는 임시 결과 표시

## 나머지 Scene 공통 조건

- [ ] 고유 GUID와 공통 GUID 분리
- [ ] 공통 Gameplay 기반 재사용
- [ ] StageData의 map, quest, fish, BGM 연결
- [ ] Missing Script/Reference 기록과 해소
- [ ] Scene 로드/종료/복귀 확인
- [ ] 원본과 다른 부분의 provenance 기록

## 결과 기록

- 정상 경로의 `Bootstrap.unity`, `LoadingScene.unity` 생성
- `BootstrapController`, `LoadingController`, `SceneSession`, `GameLog` 구현
- Editor 생성/검증 도구 `SceneSetup` 구현
- Unity `2022.3.62f3` batch compile 및 Scene component 검증 성공
- Unity Editor가 두 Scene을 실제로 열고 필수 컴포넌트를 찾는 검증 성공
- 이전 격리형 배치 정책을 폐기하고 `_ReferenceTemp`를 임시 참조 전용으로 지정

## 위험과 되돌리기

- Scene별 변경과 해당 의존 파일을 함께 기록해 다른 Scene과 섞이지 않게 한다.
- 문제가 생기면 해당 Scene과 그 작업에서 추가한 정상 경로의 의존 파일만 되돌린다.
- 원본 증거는 계속 외부 읽기 전용 폴더에 유지한다.
- 외부 package 버전이 불명확하면 임의 설치하지 않고 compatibility 또는 Placeholder로 보류한다.
