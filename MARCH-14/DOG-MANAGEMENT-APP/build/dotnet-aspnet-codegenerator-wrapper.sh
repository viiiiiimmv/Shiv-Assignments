#!/bin/bash

set -euo pipefail

script_dir="$(cd -- "$(dirname -- "$0")" && pwd)"
tool_root="$script_dir/.store/dotnet-aspnet-codegenerator"
dll_path="$(find "$tool_root" -path '*/tools/net*/any/dotnet-aspnet-codegenerator.dll' | head -n 1)"

if [[ -z "$dll_path" ]]; then
  echo "Unable to locate dotnet-aspnet-codegenerator.dll under $tool_root" >&2
  exit 1
fi

translated_args=()

for arg in "$@"; do
  case "$arg" in
    --useSqlite)
      translated_args+=(--databaseProvider sqlite)
      ;;
    --useSqlServer)
      translated_args+=(--databaseProvider sqlserver)
      ;;
    *)
      translated_args+=("$arg")
      ;;
  esac
done

exec dotnet "$dll_path" "${translated_args[@]}"
