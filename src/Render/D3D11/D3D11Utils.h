#pragma once

#include <d3d11.h>
#include <string>

namespace Euphoria::Render::D3D11::D3D11Utils {
    void CheckResult(HRESULT result, const std::string& operation);
    void CheckResult(HRESULT result);
}