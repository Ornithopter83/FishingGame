# Project Setup Guide

## 목표 환경

- 우선 검증 대상: Unity `2022.3.62f3`
- 원본 비교 환경: Unity `2022.3.2f1`
- 새 프로젝트: `UnityProject/`
- 원본 보존 자료: `ExportedProject/`, `AuxiliaryFiles/`

## 현재 확인된 환경

- 설치됨: `2022.3.2f1`, `6000.3.17f1`
- 미설치: `2022.3.62f3`
- `ExportedProject/ProjectSettings/ProjectVersion.txt`에는 `2022.3.62f3`가 기록되어 있다.
- 마스터 가이드는 원본 확인 버전을 `2022.3.2f1`로 설명한다. Export 산출 시 버전이 바뀌었을 가능성이 있어 검증 전 확정하지 않는다.

## 새 프로젝트 열기 전 절차

1. Unity Hub에서 `2022.3.62f3`를 설치한다.
2. `UnityProject/`만 Unity Hub에 추가한다.
3. `ExportedProject/`를 Unity로 직접 열지 않는다.
4. 첫 실행 전 현재 `UnityProject/Assets`, `Packages`, `ProjectSettings`를 보존한다.
5. Unity가 생성한 설정 및 패키지 변경을 기록한다.
6. 빈 프로젝트가 오류 없이 열리는지 확인한 다음에만 원본 에셋 이관 task를 시작한다.

## 버전 검증 기준

- 에디터 정상 로드
- 패키지 해석 성공
- 빈 Bootstrap/Test Scene 저장 및 재로드
- 스크립트 컴파일 오류 없음
- 원본 이관 후 주요 Material/Shader/Animator 확인
- 네이티브 DLL과 시리얼 코드가 에디터를 중단시키지 않음

Windows 빌드는 마스터 가이드의 검증 항목이지만, 사용자 지시에 따라 별도 허가 전까지 수행하지 않는다.

## 권장 Assembly Definition

초기 기능이 안정된 뒤 아래 순서로 분리한다.

- `FishingGame.Core`
- `FishingGame.Gameplay`
- `FishingGame.UI`
- `FishingGame.Hardware`
- `FishingGame.Editor`
- `FishingGame.Tests.EditMode`
- `FishingGame.Tests.PlayMode`

