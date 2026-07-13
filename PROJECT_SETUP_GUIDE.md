# Project Setup Guide

## 목표 환경

- 우선 검증 버전: Unity `2022.3.62f3`
- 원본 비교 버전: Unity `2022.3.2f1`
- 새 프로젝트: `UnityProject/`
- 읽기 전용 원본 자료: `ExportedProject/`, `AuxiliaryFiles/`

## 프로젝트 열기

1. Unity Hub에 `UnityProject/`만 추가한다.
2. `ExportedProject/`는 Unity 프로젝트로 직접 열지 않는다.
3. Unity가 만든 설정 및 패키지 변경을 확인한다.
4. Console compile/import 오류가 없는 상태에서 Scene 단위 복원을 진행한다.

## Assets 기본 구조

```text
UnityProject/Assets/
├─ Animations/
├─ Art/
├─ Audio/
├─ Materials/
├─ Models/
├─ Plugins/
├─ Prefabs/
├─ Scenes/
├─ ScriptableObjects/
├─ Scripts/
├─ ThirdParty/
└─ _ReferenceTemp/        # 선택적 임시 참조 자료, 제품 의존 금지
```

복원한 실제 파일은 용도에 맞는 정상 경로에 배치한다. `_ReferenceTemp`는 원본 조사, 비교, 변환 중 잠깐 필요한 복사본만 두며 Git에는 README와 폴더 메타만 남긴다.

## 버전 검증 기준

- 에디터 정상 로드와 패키지 해석 성공
- Bootstrap/Loading Scene 저장 및 재로딩
- 스크립트 컴파일 오류 없음
- 원본 자산 추가 시 Material/Shader/Animator 참조 확인
- DLL과 소스 코드의 에디터 충돌 없음

Windows 빌드는 별도 요청 전까지 수행하지 않는다.

## 권장 Assembly Definition

초기 기능이 안정된 뒤 필요에 따라 다음 경계로 분리한다.

- `FishingGame.Core`
- `FishingGame.Gameplay`
- `FishingGame.UI`
- `FishingGame.Hardware`
- `FishingGame.Editor`
- `FishingGame.Tests.EditMode`
- `FishingGame.Tests.PlayMode`

