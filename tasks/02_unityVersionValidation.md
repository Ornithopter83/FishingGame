# 02. Unity 버전 검증

## 상태

- R1 완료

## 목표

- 새 `UnityProject/`가 Unity `2022.3.62f3`에서 안전하게 열리고 Scene을 저장·재로드할 수 있는지 확인한다.

## 결과

- Unity `2022.3.62f3` 설치 및 batch editor 실행 확인
- package manifest와 lock file 해석 성공
- `Assembly-CSharp`, `Assembly-CSharp-Editor` 컴파일 성공
- `Bootstrap.unity`, `LoadingScene.unity` 저장·재로드 및 필수 컴포넌트 확인
- Build Settings에 두 Scene이 활성 경로로 등록됨
- 하드웨어와 외부 서비스 없이 Scene을 열 수 있음
- Windows 빌드는 정책에 따라 수행하지 않음

## 완료 조건

- [x] 2022.3.62f3 설치
- [x] 새 프로젝트 에디터 로드
- [x] 패키지/Console 상태 기록
- [x] 기본 Scene 저장/재로드
- [x] 버전 선택 근거 갱신
- [x] Windows 빌드는 사용자 승인 전 미수행

## 버전 선택 근거

- 마스터 가이드 우선 버전과 `ExportedProject/ProjectSettings/ProjectVersion.txt`가 `2022.3.62f3`을 가리킨다.
- 현재 기본 Scene과 패키지가 이 버전에서 오류 없이 import/compile된다.
- `2022.3.2f1`은 비교 환경으로만 유지한다.

