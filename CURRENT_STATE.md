# Current State

업데이트: 2026-07-13

## 현재 단계

- Phase 0 기초 정보와 원본 구조 조사 완료
- Phase 1 Scene 단위 복원 진행 중
- 복원 수준: 구조 R1, Bootstrap/Loading 기본 흐름 R2
- Unity 검증 버전: `2022.3.62f3`
- Git: 사용자가 첫 커밋과 `origin/main` 푸시 완료
- 이번 작업에서 빌드, 커밋, 푸시 미수행

## 기본 정책

- `ExportedProject/`, `AuxiliaryFiles/`는 읽기 전용 원본 증거다.
- 실제 코드와 자산은 `UnityProject/Assets`의 정상 제작 폴더에 배치한다.
- `_ReferenceTemp`는 선택적인 임시 참조 영역이며 런타임 의존을 허용하지 않는다.
- 출처와 추정 상태는 문서와 task에 기록하고 제작 이름에 `Recovery` 접두어를 붙이지 않는다.

## 완료한 내용

- `Docs/ORIGINAL_STRUCTURE_MAP.md`에 핵심 코드, Scene 역할, Gameplay map 구조 지도 작성
- 핵심 C# 8개와 `Assembly-CSharp.dll`의 공개 member 이름 대응 확인
- Build Settings 원본 Scene `Title`, `LoadingScene`, `LastScene` 확인
- Addressables Scene 12개 중 선택 Scene 1개와 Gameplay Scene 11개의 역할/지역명 매핑
- 정상 구조 `Assets/Scripts`, `Scenes`, `Prefabs`, `Art`, `Audio` 등 생성
- `Bootstrap.unity`, `LoadingScene.unity`와 기본 Scene 흐름 구현
- `BootstrapController`, `LoadingController`, `SceneSession`, `GameLog`, `SceneSetup` 구현
- 이전 격리형 제작 경로를 제거하고 `_ReferenceTemp` 임시 참조 정책으로 전환
- 원본 17,701개 파일의 전체 SHA-256 manifest 생성 및 원본 불변 확인
- GUID/YAML 참조 감사 완료: 중복 GUID 0, 미해결 Scene/Prefab script 참조 0
- Unity Editor에서 Bootstrap과 Loading Scene 저장·재로드 확인

## 원본 현황

- `ExportedProject/Assets`: 17,539개 파일, 약 16.75GB
- 주요 수량: C# 362, Scene 15, Prefab 316, `.asset` 3,289, Material 1,121, Shader 272
- `StreamingAssets/aa`: 약 5.3GB의 Windows Addressables 빌드 산출물 포함
- 핵심 대상 `.meta`: `StateManager`, `FishGameManager`, `StageManager`, `DataManager`, `RodCont`, `TensionCont`

## 다음 작업

1. 사용자가 Bootstrap Scene을 열어 화면과 Bootstrap → Loading → Bootstrap 흐름을 Play Mode에서 확인
2. Loading Scene 직접/간접 asset closure와 패키지 경계를 확정
3. 원본 Loading layout을 정상 경로에 필요한 자산만 가져와 복원
4. Busan Gameplay Scene을 기준으로 공통 Gameplay 수직 조각 복원
5. Result/Last와 Title을 순차 복원하고 최종 흐름을 연결

## 남은 불확실성

- 일부 C# 소스의 정확한 provenance
- 해시형 Scene 12개의 원래 파일명 일부
- 외부 Asset Store 패키지의 정확한 버전과 라이선스
- Addressables catalog의 원본 주소/레이블 복원 가능 범위
- 실제 하드웨어 기반 R5 검증 조건
