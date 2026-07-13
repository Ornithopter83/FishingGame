# 01. 원본 보존 및 Manifest

## 상태

- R1 완료 (외부 물리 백업 위치만 사용자 지정 대기)

## 목표

- `ExportedProject/`와 `AuxiliaryFiles/`를 불변 기준 자료로 확정한다.
- 경로, 크기, 수정 시각, SHA-256을 포함한 재생성 가능한 manifest를 만든다.

## 결과

- 전체 17,701개 파일, 16,783,310,435바이트를 기록했다.
- `Docs/ORIGINAL_ARCHIVE_MANIFEST.csv`에 파일별 SHA-256을 생성했다.
- Manifest 자체 SHA-256은 `3da7a17203435ee54af0d379e981a3163d766cf41716bc9078ea65ccbbffa40b`다.
- 생성 전후 원본 파일 수, 총 크기, 최신 수정 시각이 동일함을 확인했다.
- 재생성 도구는 `Tools/New-ArchiveManifest.ps1`이다.
- 상세 요약은 `Docs/ORIGINAL_ARCHIVE_MANIFEST_SUMMARY.md`에 기록했다.

## 완료 조건

- [x] 원본 전체 파일 목록 생성
- [x] 모든 파일의 SHA-256 생성
- [x] `.meta`, ProjectVersion, manifest 존재와 해시 기록
- [x] 원본/작업 프로젝트 경계 문서화
- [x] 별도 백업 필요 용량 산정: 최소 15.63GiB + 파일시스템 여유 공간
- [ ] 별도 물리 백업 위치 지정 및 복제: 사용자 저장 장치 지정 필요

## 변경 금지

- 원본 파일 이동, 이름 변경, 수정, 삭제 금지
- manifest를 원본 폴더 안에 생성하지 않음

