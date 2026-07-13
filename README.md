# Fishing Game Recovery

조각난 Unity 낚시 게임을 원본 증거에 기반해 새 프로젝트로 복구하는 작업 공간이다.

## 디렉터리 역할

- `ExportedProject/`: 원본 보존 영역. 직접 수정하지 않는다.
- `AuxiliaryFiles/`: DLL 및 보조 분석 자료. 직접 수정하지 않는다.
- `UnityProject/`: 새로 구성하는 Unity 프로젝트.
- `Docs/`: 분석 결과, 의사결정, 상태 흐름 문서.
- `tasks/`: 작은 단위의 조사·복구 작업 기록.

## 현재 범위

Phase 0 조사와 프로젝트 골격만 시작했다. 게임 기능, 원본 에셋 가져오기, Unity 실행, 빌드는 아직 수행하지 않았다. 자세한 상태는 `CURRENT_STATE.md`를 참조한다.

## 핵심 원칙

1. 원본 자료를 수정하거나 삭제하지 않는다.
2. 확인된 사실과 추정을 구분한다.
3. 새 구현은 `Original`, `Recovered`, `Recreated`, `Placeholder`, `Unknown`으로 출처를 표시한다.
4. Scene/Prefab/ScriptableObject 변경 전에 GUID와 참조 관계를 조사한다.
5. 명시적 요청 전에는 빌드, 커밋, 풀, 푸시하지 않는다.

