/*
   Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
   SPDX-License-Identifier: Apache-2.0
*/

package com.aws.rest;

import jxl.write.WriteException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import java.io.IOException;
import java.io.InputStream;
import java.util.Map;


@CrossOrigin(origins = "*")
@RestController
@RequestMapping("api/items:report")
public class ReportController {

    private final WorkItemRepository repository;

    private final WriteExcel writeExcel;

    private final WriteExcel.SendMessages sm;
    @Autowired()
    ReportController(
        WorkItemRepository repository,
        WriteExcel writeExcel,
        WriteExcel.SendMessages sm
    ) {
        this.repository = repository;
        this.writeExcel = writeExcel;
        this.sm = sm;
    }

    @PostMapping("")
    public String sendReport(@RequestBody Map<String, String> body) {
        var list = repository.findAllWithStatus("false");
        try {
            InputStream is = writeExcel.write(list);
            sm.sendReport(is, body.get("email"));
            return "Report generated & sent";
        } catch (IOException | WriteException e) {
            e.printStackTrace();
        }
        return "Failed to generate report";
    }
}