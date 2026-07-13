# Current State

업데이트: 2026-07-13

## 현재 단계

- Phase 0: 원본 보존 및 조사
- 복원 수준: R1(구조 확인) 착수
- 빌드/Unity 실행: 미수행
- 커밋/푸시: 미수행

## 이번에 완료한 내용

- 복구 마스터 가이드 검토
- `ExportedProject/`와 `AuxiliaryFiles/`를 원본 보존 영역으로 지정
- 새 `UnityProject/` 디렉터리와 `2022.3.62f3` 목표 골격 생성
- 복구 정책, 상태 흐름, 에셋 현황, DLL 분석 초안 및 Phase 0 task 생성
- GitHub 원격 `Ornithopter83/FishingGame.git`이 접근 가능하지만 현재 ref가 없는 빈 저장소임을 읽기 전용으로 확인
- 로컬 Git 저장소를 `main`으로 초기화하고 위 주소를 `origin`으로 연결(커밋/풀/푸시 없음)

## 확인된 원본 현황

- `ExportedProject/Assets`: 17,539개 파일, 약 16.75 GB
- 주요 수량: `.cs` 362, `.unity` 15, `.prefab` 316, `.asset` 3,289, `.mat` 1,121, `.shader` 272
- `.meta` 없음: 파일 기준 237개(초기 정적 조사)
- Build Settings: `Title`, `LoadingScene`, `LastScene` 3개 활성
- 핵심 타입 소스 존재: `StateManager`, `FishGameManager`, `StageManager`, `DataManager`, `RodCont`, `TensionCont`
- 목표 Unity `2022.3.62f3`은 로컬에 설치되어 있지 않음

## 변경하지 않은 범위

- `ExportedProject/` 전체
- `AuxiliaryFiles/` 전체
- 원본 Scene/Prefab/ScriptableObject 및 `.meta`
- 게임 기능과 에셋 연결

## 다음 권장 작업

1. `tasks/01_originalArchive.md`: 전체 파일 manifest와 SHA-256 산출 방식을 결정하고 원본 보존 상태 확정
2. `tasks/02_unityVersionValidation.md`: 62f3 설치 후 빈 새 프로젝트 호환성 검증(빌드는 별도 승인 필요)
3. `tasks/03_assetInventory.md`: GUID/참조/Missing Script/Shader 누락 자동 조사
4. 이후 `04_dllDeepAnalysis.md`와 Scene/Build inventory 작성

## 차단/불확실성

- 원본이 실제로 마지막 실행된 Unity 버전은 아직 확정되지 않았다.
- C# 소스가 원본 소스인지 DLL 역추출 결과인지 provenance 확인이 필요하다.
- 16.75 GB 원본 전체 복제는 중복 저장 비용 때문에 아직 수행하지 않았다.
- Unity를 실행하지 않아 컴파일, import, Missing Reference 상태는 검증되지 않았다.
