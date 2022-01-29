#UTLWO2B.jcl  Loan Sale (rewrite) Trigger and Reporting
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO2B.LWO2BR1
   then
        rm ${reportdir}/ULWO2B.LWO2BR1
fi
if test -a ${reportdir}/ULWO2B.LWO2BR2
   then
        rm ${reportdir}/ULWO2B.LWO2BR2
fi
if test -a ${reportdir}/ULWO2B.LWO2BR10
   then
        rm ${reportdir}/ULWO2B.LWO2BR10
fi
if test -a ${reportdir}/ULWO2B.LWO2BR11
   then
        rm ${reportdir}/ULWO2B.LWO2BR11
fi
if test -a ${reportdir}/ULWO2B.LWO2BR12
   then
        rm ${reportdir}/ULWO2B.LWO2BR12
fi
if test -a ${reportdir}/ULWO2B.LWO2BR13
   then
        rm ${reportdir}/ULWO2B.LWO2BR13
fi
if test -a ${reportdir}/ULWO2B.LWO2BR14
   then
        rm ${reportdir}/ULWO2B.LWO2BR14
fi
if test -a ${reportdir}/ULWO2B.LWO2BR15
   then
        rm ${reportdir}/ULWO2B.LWO2BR15
fi
if test -a ${reportdir}/ULWO2B.LWO2BR16
   then
        rm ${reportdir}/ULWO2B.LWO2BR16
fi
if test -a ${reportdir}/ULWO2B.LWO2BR30
   then
        rm ${reportdir}/ULWO2B.LWO2BR30
fi
if test -a ${reportdir}/ULWO2B.LWO2BR31
   then
        rm ${reportdir}/ULWO2B.LWO2BR31
fi
if test -a ${reportdir}/ULWO2B.LWO2BR32
   then
        rm ${reportdir}/ULWO2B.LWO2BR32
fi
if test -a ${reportdir}/ULWO2B.LWO2BR33
   then
        rm ${reportdir}/ULWO2B.LWO2BR33
fi
if test -a ${reportdir}/ULWO2B.LWO2BR34
   then
        rm ${reportdir}/ULWO2B.LWO2BR34
fi
if test -a ${reportdir}/ULWO2B.LWO2BR50
   then
        rm ${reportdir}/ULWO2B.LWO2BR50
fi
if test -a ${reportdir}/ULWO2B.LWO2BR51
   then
        rm ${reportdir}/ULWO2B.LWO2BR51
fi
if test -a ${reportdir}/ULWO2B.LWO2BR52
   then
        rm ${reportdir}/ULWO2B.LWO2BR52
fi
if test -a ${reportdir}/ULWO2B.LWO2BR53
   then
        rm ${reportdir}/ULWO2B.LWO2BR53
fi
if test -a ${reportdir}/ULWO2B.LWO2BR54
   then
        rm ${reportdir}/ULWO2B.LWO2BR54
fi
if test -a ${reportdir}/ULWO2B.LWO2BR70
   then
        rm ${reportdir}/ULWO2B.LWO2BR70
fi
if test -a ${reportdir}/ULWO2B.LWO2BR71
   then
        rm ${reportdir}/ULWO2B.LWO2BR71
fi
if test -a ${reportdir}/ULWO2B.LWO2BR72
   then
        rm ${reportdir}/ULWO2B.LWO2BR72
fi
if test -a ${reportdir}/ULWO2B.LWO2BR73
   then
        rm ${reportdir}/ULWO2B.LWO2BR73
fi
if test -a ${reportdir}/ULWO2B.LWO2BR74
   then
        rm ${reportdir}/ULWO2B.LWO2BR74
fi
if test -a ${reportdir}/ULWO2B.LWO2BR75
   then
        rm ${reportdir}/ULWO2B.LWO2BR75
fi
if test -a ${reportdir}/ULWO2B.LWO2BR90
   then
        rm ${reportdir}/ULWO2B.LWO2BR90
fi

# run the program

sas ${codedir}/UTLWO2B.sas -log ${reportdir}/ULWO2B.LWO2BR1  -mautosource

# set full group/non-group permissions for lock-down file and trigger file

chmod 666 /sas/whse/olrp_lookup_directory/utlwo2b.sas7bdat

if test -a ${reportdir}/ULWO2B.LWO2BR2
   then
	chmod 666 ${reportdir}/ULWO2B.LWO2BR2
fi

