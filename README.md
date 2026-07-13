# Fishing Game

조각난 Unity 낚시 게임 자료를 근거로 새 Unity 프로젝트를 재구성하는 작업 공간이다.

## 디렉터리

- `ExportedProject/`: 읽기 전용 원본 증거 영역
- `AuxiliaryFiles/`: DLL 및 보조 분석 자료
- `UnityProject/`: 실제로 재구성하는 Unity 프로젝트
- `Docs/`: 분석 결과와 구조 지도
- `tasks/`: 작업 단위별 범위와 결과

실제 게임 파일은 `UnityProject/Assets`의 정상 폴더 구조에 둔다. 임시 참조 자료가 필요할 때만 `_ReferenceTemp`를 사용하며 제품 코드나 Scene은 이 폴더에 의존하지 않는다.

자세한 현재 상태는 `CURRENT_STATE.md`, 작업 규칙은 `AGENTS.md`를 참조한다. 명시적인 요청 전에는 빌드, 커밋, 푸시를 수행하지 않는다.

